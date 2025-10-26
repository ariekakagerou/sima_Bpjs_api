# 🚀 QUICK START - Security Enhanced SIMA BPJS API

## ⚡ CEPAT - Langkah Instalasi (5 Menit)

### Step 1: Install Packages (30 detik)

```powershell
cd sima_Bpjs_api

dotnet add package AspNetCoreRateLimit
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console

dotnet restore
```

### Step 2: Database Migration (1 menit)

```powershell
# Buat migration
dotnet ef migrations add AddSecurityEnhancements

# Apply ke database
dotnet ef database update
```

### Step 3: Setup Environment Variables (30 detik)

```powershell
# Jalankan script setup
.\setup-environment.ps1

# Atau manual:
$env:SIMA_JWT_SECRET = "SuPerSecReT-JWT-KeY-F0r-BPJS-ApI-2024-M1n1mAl-32-ChArS-L0nG!"
$env:SIMA_DB_CONNECTION = "server=localhost;port=3306;database=sima_bpjs;user=root;password=;"
```

### Step 4: Jalankan API (10 detik)

```powershell
dotnet run
```

**API berjalan di:** `https://localhost:7XXX` (check console)

### Step 5: Test (2 menit)

**Test Password Validation:**
```bash
# Harus DITOLAK (password lemah)
curl -X POST https://localhost:7xxx/api/auth/register ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"test\",\"password\":\"weak\",\"email\":\"test@test.com\"}"
```

**Test Rate Limiting:**
```bash
# Request ke-6 dalam 1 menit = DITOLAK (429)
for /L %i in (1,1,6) do (
  curl -X POST https://localhost:7xxx/api/auth/login ^
    -H "Content-Type: application/json" ^
    -d "{\"emailOrPhone\":\"test\",\"password\":\"test\"}"
)
```

**Test Refresh Token:**
```bash
# 1. Login (dapatkan tokens)
curl -X POST https://localhost:7xxx/api/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"emailOrPhone\":\"admin_bpjs\",\"password\":\"Admin@BPJS2024!\"}"

# 2. Simpan refreshToken dari response
# 3. Refresh access token
curl -X POST https://localhost:7xxx/api/auth/refresh ^
  -H "Content-Type: application/json" ^
  -d "{\"refreshToken\":\"YOUR_REFRESH_TOKEN_HERE\"}"
```

---

## ✅ SECURITY FEATURES YANG AKTIF

| Fitur | Status | Deskripsi |
|-------|--------|-----------|
| **Rate Limiting** | ✅ Active | Max 5 login/minute, 100 req/minute |
| **Account Lockout** | ✅ Active | Lock 15 min setelah 5x failed login |
| **Password Validation** | ✅ Active | Min 8 char, uppercase, lowercase, number, special |
| **Refresh Tokens** | ✅ Active | 60 min access token, 7 day refresh token |
| **Security Headers** | ✅ Active | XSS, Clickjacking, CSRF protection |
| **Secrets Management** | ✅ Active | Environment variables untuk JWT key & DB |
| **Audit Logging** | ✅ Active | All requests logged ke `logs/` folder |

---

## 📊 DEFAULT CREDENTIALS

**Admin Account:**
- Username: `admin_bpjs`
- Password: `Admin@BPJS2024!`

---

## 🔧 TROUBLESHOOTING

### ❌ Error: "Database connection not configured"
**Solusi:**
```powershell
$env:SIMA_DB_CONNECTION = "server=localhost;port=3306;database=sima_bpjs;user=root;password=YOUR_PASSWORD"
```

### ❌ Error: "JWT Key not configured"
**Solusi:**
```powershell
$env:SIMA_JWT_SECRET = "minimum-32-characters-long-secret-key-here"
```

### ❌ Error: Migration pending
**Solusi:**
```powershell
dotnet ef database update
```

### ❌ Error: AspNetCoreRateLimit not found
**Solusi:**
```powershell
dotnet add package AspNetCoreRateLimit
dotnet restore
```

---

## 📁 FILES YANG BERUBAH

**Baru:**
- ✅ `Validators/PasswordValidator.cs`
- ✅ `models/RefreshToken.cs`
- ✅ `setup-environment.ps1`
- ✅ `SECURITY_IMPLEMENTATION_GUIDE.md`

**Diupdate:**
- ✅ `models/User.cs` (added lockout fields)
- ✅ `Data/AppDbContext.cs` (added RefreshTokens)
- ✅ `controllers/AuthController.cs` (all security features)
- ✅ `Program.cs` (rate limiting, headers, logging)

**Backup:**
- `controllers/AuthController_OLD_BACKUP.cs`
- `Program_OLD_BACKUP.cs`

---

## 🎯 NEXT STEPS

1. **Test semua endpoints** - Pastikan tidak ada yang error
2. **Update frontend** - Implement refresh token flow
3. **Adjust rate limits** - Sesuaikan dengan usage patterns
4. **Monitor logs** - Check `logs/sima-bpjs-*.log`
5. **Production setup** - Set environment variables as system vars

---

## 🆘 ROLLBACK

Jika ada masalah:
```powershell
# 1. Restore backup files
copy controllers\AuthController_OLD_BACKUP.cs controllers\AuthController.cs
copy Program_OLD_BACKUP.cs Program.cs

# 2. Rollback migration
dotnet ef database update PreviousMigrationName

# 3. Restart
dotnet run
```

---

## 📚 DOKUMENTASI LENGKAP

- Full Guide: `SECURITY_IMPLEMENTATION_GUIDE.md`
- Package Install: `SECURITY_PACKAGES_INSTALL.md`

---

**Version:** 2.0 Enhanced Security  
**Setup Time:** ~5 minutes  
**Ready for:** Development & Testing

