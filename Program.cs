using Microsoft.EntityFrameworkCore;
using sima_bpjs_api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// pakai AppDbContext, bukan SimaBpjsContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not configured")))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SIMA BPJS API", Version = "v1" });
    
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
    client.BaseAddress = new Uri("https://ktp.chasouluix.biz.id/");
});

// Tambahkan konfigurasi CORS agar bisa diakses dari frontend React/Next.js
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // ganti jika frontend di domain lain
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

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
            PasswordHash = HashPassword("admin123"),
            Role = "ADMIN",
            Nik = "3201012345670001",
            CreatedAt = DateTime.UtcNow
        };
        
        context.Users.Add(admin);
        await context.SaveChangesAsync();
        Console.WriteLine("✅ Admin berhasil dibuat: admin_bpjs / admin123");
    }
    else
    {
        Console.WriteLine("ℹ️ Admin sudah ada di database");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // Aktifkan CORS sebelum authentication
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

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
