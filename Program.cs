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

    // ✅ Enhanced: Auto resolve connection string for local, Docker, Railway, or Render
    var connectionString = ResolveConnectionString(builder.Configuration);

    var jwtKey = Environment.GetEnvironmentVariable("SIMA_JWT_SECRET")
        ?? builder.Configuration["Jwt:Key"]
        ?? "supersecretkey123456789012345678901234567890";

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
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";

        // Prevent clickjacking
        context.Response.Headers["X-Frame-Options"] = "DENY";

        // XSS Protection (legacy but still useful)
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

        // HTTPS Strict Transport Security
        if (context.Request.IsHttps)
        {
            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
        }

        // Content Security Policy
        context.Response.Headers["Content-Security-Policy"] =
            "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self'; connect-src 'self'; frame-ancestors 'none';";

        // Referrer Policy
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        // Permissions Policy
        context.Response.Headers["Permissions-Policy"] =
            "geolocation=(), microphone=(), camera=(), payment=(), usb=(), magnetometer=()";

        // Remove server header
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");

        await next();
    });

    // ✅ Forwarded Headers for Cloud / Reverse Proxy (Railway, Render, Docker)
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
    });

    // Auto-create database schema and seed initial data
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Auto-create tables if database is fresh
            await context.Database.EnsureCreatedAsync();
            Log.Information("✅ Database schema verified / created successfully");

            // Seed Admin user
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
                Log.Information("✅ Admin default berhasil dibuat: admin_bpjs / Admin@BPJS2024!");
            }

            // Seed Demo regular user
            var userExists = await context.Users.AnyAsync(u => u.Role == "USER");
            if (!userExists)
            {
                var demoUser = new sima_bpjs_api.Models.User
                {
                    Username = "user_biasa",
                    PasswordHash = HashPassword("User@BPJS2024!"),
                    Role = "USER",
                    Nik = "3201012345670002",
                    CreatedAt = DateTime.UtcNow,
                    FailedLoginAttempts = 0
                };

                context.Users.Add(demoUser);
                await context.SaveChangesAsync();
                Log.Information("✅ Demo User berhasil dibuat: user_biasa / User@BPJS2024!");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "⚠️ Database initialization/migration notice: {Message}", ex.Message);
        }
    }

    // Configure Swagger (Enabled in Dev or when ENABLE_SWAGGER != "false")
    var enableSwaggerEnv = Environment.GetEnvironmentVariable("ENABLE_SWAGGER");
    var isSwaggerEnabled = app.Environment.IsDevelopment() ||
                           string.Equals(enableSwaggerEnv, "true", StringComparison.OrdinalIgnoreCase) ||
                           string.IsNullOrEmpty(enableSwaggerEnv);

    if (isSwaggerEnabled)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIMA BPJS API v2.0");
            c.RoutePrefix = "swagger";
        });
        Log.Information("Swagger UI enabled at /swagger");

        // Redirect root '/' directly to '/swagger' for recruiters convenience
        app.MapGet("/", () => Results.Redirect("/swagger"));
    }

    // ✅ SECURITY: Request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        };
    });

    if (!app.Environment.IsDevelopment())
    {
        // Only redirect in non-container/direct HTTPS
        app.UseHttpsRedirection();
    }

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

// Helper method untuk resolusi connection string (Local, Docker, Railway, Render)
static string ResolveConnectionString(IConfiguration configuration)
{
    // 1. Explicit connection string env var
    var explicitConn = Environment.GetEnvironmentVariable("SIMA_DB_CONNECTION")
        ?? Environment.GetEnvironmentVariable("DATABASE_URL");

    // 2. Railway MYSQL_URL format: mysql://user:password@host:port/database
    var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
    if (!string.IsNullOrWhiteSpace(mysqlUrl))
    {
        try
        {
            var uri = new Uri(mysqlUrl);
            var userInfo = uri.UserInfo.Split(':');
            var user = userInfo.Length > 0 ? userInfo[0] : "root";
            var pass = userInfo.Length > 1 ? userInfo[1] : "";
            var db = uri.AbsolutePath.TrimStart('/');
            return $"server={uri.Host};port={uri.Port};database={db};user={user};password={pass};";
        }
        catch (Exception ex)
        {
            Log.Warning("Could not parse MYSQL_URL ({Error}), falling back...", ex.Message);
        }
    }

    // 3. Railway / Cloud individual environment variables
    var host = Environment.GetEnvironmentVariable("MYSQLHOST");
    if (!string.IsNullOrWhiteSpace(host))
    {
        var port = Environment.GetEnvironmentVariable("MYSQLPORT") ?? "3306";
        var user = Environment.GetEnvironmentVariable("MYSQLUSER") ?? "root";
        var pass = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? "";
        var db = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? "railway";
        return $"server={host};port={port};database={db};user={user};password={pass};";
    }

    if (!string.IsNullOrWhiteSpace(explicitConn))
    {
        return explicitConn;
    }

    // 4. Fallback to appsettings.json DefaultConnection
    return configuration.GetConnectionString("DefaultConnection")
        ?? "server=localhost;port=3306;database=sima_bpjs;user=root;password=;";
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

