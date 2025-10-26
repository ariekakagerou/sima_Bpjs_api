# 📦 Install Security Packages

Jalankan command berikut di terminal (PowerShell) di folder `sima_Bpjs_api`:

```powershell
# 1. Rate Limiting
dotnet add package AspNetCoreRateLimit

# 2. Logging (Serilog)
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console

# 3. Restore packages
dotnet restore
```

## Verifikasi Installation

Setelah install, cek file `sima_Bpjs_api.csproj` harus ada:
- AspNetCoreRateLimit
- Serilog.AspNetCore
- Serilog.Sinks.File
- Serilog.Sinks.Console

## Next Steps

Setelah package terinstall, jalankan:
```powershell
# Create migration untuk Account Lockout dan Refresh Token
dotnet ef migrations add AddSecurityEnhancements

# Update database
dotnet ef database update
```

