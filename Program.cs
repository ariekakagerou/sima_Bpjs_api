using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AspNetCoreRateLimit;
using Serilog;
using Serilog.Events;

// ✅ SECURITY ENHANCEMENT: Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/sima-bpjs-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 30
    )
    .CreateLogger();

try
{
    Log.Information("Starting SIMA BPJS API...");

    var builder = WebApplication.CreateBuilder(args);

    // ✅ SECURITY: Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllers();

    // ✅ SECURITY ENHANCEMENT: Load secrets from environment variables
    var connectionString = Environment.GetEnvironmentVariable("SIMA_DB_CONNECTION")
        ?? builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Database connection string not configured");

    var jwtKey = Environment.GetEnvironmentVariable("SIMA_JWT_SECRET")
        ?? builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("JWT Key not configured");

    Log.Information("Configuration loaded successfully");

    // Configure Database
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
        )
    );

    // ✅ SECURITY ENHANCEMENT: Rate Limiting Configuration
    builder.Services.AddMemoryCache();
    builder.Services.Configure<IpRateLimitOptions>(options =>
    {
        options.EnableEndpointRateLimiting = true;
        options.StackBlockedRequests = false;
        options.HttpStatusCode = 429;
        options.RealIpHeader = "X-Real-IP";
        options.ClientIdHeader = "X-ClientId";
        options.GeneralRules = new List<RateLimitRule>
        {
            // Login endpoint - strict rate limiting
            new RateLimitRule
            {
                Endpoint = "POST:/api/auth/login",
                Period = "1m",
                Limit = 5 // Max 5 login attempts per minute per IP
            },
            new RateLimitRule
            {
                Endpoint = "POST:/api/auth/login",
                Period = "1h",
                Limit = 20 // Max 20 login attempts per hour per IP
            },
            // Register endpoint
            new RateLimitRule
            {
                Endpoint = "POST:/api/auth/register",
                Period = "1h",
                Limit = 3 // Max 3 registrations per hour per IP
            },
            // Refresh token endpoint
            new RateLimitRule
            {
                Endpoint = "POST:/api/auth/refresh",
                Period = "1m",
                Limit = 10
            },
            // General API rate limit
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1m",
                Limit = 100 // Max 100 requests per minute for other endpoints
            },
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1h",
                Limit = 1000
            }
        };
    });

    builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

    // Configure JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero // Remove default 5 minute clock skew
        };

        // Log JWT authentication events
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Warning("JWT Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var username = context.Principal?.Identity?.Name;
                Log.Debug("JWT Token validated for user: {Username}", username);
                return Task.CompletedTask;
            }
        };
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "SIMA BPJS API - Enhanced Security",
            Version = "v2.0",
            Description = "BPJS Management API with Enterprise Security Features"
        });

        // Add JWT Authentication to Swagger
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    // HttpClient for external KTP API
    builder.Services.AddHttpClient("KtpApi", client =>
    {
        client.BaseAddress = new Uri("https://ktp-web.chasouluix.biz.id");
        client.Timeout = TimeSpan.FromSeconds(30);
    });

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
            policy =>
            {
                // ✅ SECURITY: Can be configured via environment variable for production
                var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',')
                    ?? new[] { "http://localhost:3000", "http://localhost:5173" };

                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
    });

    var app = builder.Build();

    // ✅ SECURITY ENHANCEMENT: Security Headers Middleware
    app.Use(async (context, next) =>
    {
        // Prevent MIME type sniffing
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

        // Prevent clickjacking
        context.Response.Headers.Add("X-Frame-Options", "DENY");

        // XSS Protection (legacy but still useful)
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

        // HTTPS Strict Transport Security
        if (context.Request.IsHttps)
        {
            context.Response.Headers.Add("Strict-Transport-Security",
                "max-age=31536000; includeSubDomains; preload");
        }

        // Content Security Policy
        context.Response.Headers.Add("Content-Security-Policy",
            "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self'; connect-src 'self'; frame-ancestors 'none';");

        // Referrer Policy
        context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

        // Permissions Policy
        context.Response.Headers.Add("Permissions-Policy",
            "geolocation=(), microphone=(), camera=(), payment=(), usb=(), magnetometer=()");

        // Remove server header
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");

        await next();
    });

    // Seed admin data
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Cek apakah admin sudah ada
        var adminExists = await context.Users.AnyAsync(u => u.Role == "ADMIN");
        if (!adminExists)
        {
            var admin = new sima_bpjs_api.Models.User
            {
                Username = "admin_bpjs",
                PasswordHash = HashPassword("Admin@BPJS2024!"),
                Role = "ADMIN",
                Nik = "3201012345670001",
                CreatedAt = DateTime.UtcNow,
                FailedLoginAttempts = 0
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
            Log.Information("✅ Admin berhasil dibuat: admin_bpjs / Admin@BPJS2024!");
        }
        else
        {
            Log.Information("ℹ️ Admin sudah ada di database");
        }
    }

    // Configure middleware pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        Log.Information("Swagger UI enabled (Development mode)");
    }
    else
    {
        // ✅ SECURITY: Disable Swagger in production
        Log.Information("Running in Production mode - Swagger disabled");
    }

    // ✅ SECURITY: Request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        };
    });

    app.UseHttpsRedirection();

    // ✅ SECURITY ENHANCEMENT: Rate Limiting (must be before CORS)
    app.UseIpRateLimiting();

    app.UseCors("AllowFrontend");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("SIMA BPJS API started successfully");
    Log.Information("Security features enabled: Rate Limiting, Account Lockout, Password Validation, Refresh Tokens, Security Headers");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Helper method untuk hash password
static string HashPassword(string password)
{
    using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
    var salt = new byte[16];
    rng.GetBytes(salt);

    var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(
        password, salt, 100000, System.Security.Cryptography.HashAlgorithmName.SHA256);
    var hash = pbkdf2.GetBytes(32);

    var combined = new byte[1 + salt.Length + hash.Length];
    combined[0] = 0x01; // version
    Buffer.BlockCopy(salt, 0, combined, 1, salt.Length);
    Buffer.BlockCopy(hash, 0, combined, 1 + salt.Length, hash.Length);
    return Convert.ToBase64String(combined);
}

