# 🔒 SECURITY IMPLEMENTATION GUIDE
# SIMA BPJS API - Enhanced Security Features

## 📋 Overview

API ini telah di-enhance dengan 6 security features enterprise-level:

1. ✅ **Rate Limiting** - Mencegah brute-force attacks
2. ✅ **Account Lockout** - Lock account setelah 5x failed login
3. ✅ **Password Strength Validation** - Enforce strong passwords
4. ✅ **JWT Refresh Tokens** - Long-lived sessions dengan security
5. ✅ **Secrets Management** - Environment variables untuk sensitive data
6. ✅ **Security Headers** - HTTP headers untuk berbagai protections

---

## 🚀 QUICK START - Installation Steps

### Step 1: Install Required Packages

```powershell
cd sima_Bpjs_api

# Install security packages
dotnet add package AspNetCoreRateLimit
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console

# Restore all packages
dotnet restore
```

### Step 2: Create Database Migration

```powershell
# Create migration for new security tables and columns
dotnet ef migrations add AddSecurityEnhancements

# Apply migration to database
dotnet ef database update
```

### Step 3: Setup Environment Variables

```powershell
# Run setup script
.\setup-environment.ps1

# Or manually set:
$env:SIMA_JWT_SECRET = "your-super-secret-jwt-key-min-32-chars"
$env:SIMA_DB_CONNECTION = "server=localhost;port=3306;database=sima_bpjs;user=root;password=yourpassword"
```

### Step 4: Run the API

```powershell
dotnet run
```

API akan berjalan di: `https://localhost:7XXX` (lihat console output)

---

## 📁 Files Yang Ditambahkan/Dimodifikasi

### **Files Baru:**
- `Validators/PasswordValidator.cs` - Password strength validator
- `models/RefreshToken.cs` - Refresh token model
- `setup-environment.ps1` - Environment setup script
- `SECURITY_PACKAGES_INSTALL.md` - Package installation guide
- `SECURITY_IMPLEMENTATION_GUIDE.md` - This file

### **Files Dimodifikasi:**
- `models/User.cs` - Added lockout properties
- `Data/AppDbContext.cs` - Added RefreshTokens DbSet
- `controllers/AuthController.cs` - Enhanced with all security features
- `Program.cs` - Added rate limiting, security headers, Serilog

### **Files Backup:**
- `controllers/AuthController_OLD_BACKUP.cs` - Original auth controller
- `Program_OLD_BACKUP.cs` - Original program.cs

---

## 🔐 SECURITY FEATURES DETAIL

### 1. **Rate Limiting**

**Konfigurasi di `Program.cs`:**
```csharp
// Login: Max 5 attempts per minute
POST:/api/auth/login - 5 requests/minute, 20 requests/hour

// Register: Max 3 per hour
POST:/api/auth/register - 3 requests/hour

// General API: Max 100 requests per minute
* - 100 requests/minute
```

**Testing:**
```bash
# Coba login 6x dalam 1 menit, request ke-6 akan ditolak dengan status 429
curl -X POST https://localhost:7xxx/api/auth/login
```

---

### 2. **Account Lockout**

**Rules:**
- Setelah 5x failed login attempts: account terkunci 15 menit
- Counter reset setelah successful login
- User melihat remaining lockout time

**Database Columns (users table):**
- `failed_login_attempts` INT - Counter failed attempts
- `lockout_end` DATETIME - Kapan lockout berakhir

**Testing:**
```bash
# Login dengan password salah 5x
# Attempt ke-6 akan return HTTP 423 (Locked)
```

---

### 3. **Password Strength Validation**

**Requirements:**
- ✅ Minimal 8 karakter
- ✅ 1 huruf besar (A-Z)
- ✅ 1 huruf kecil (a-z)
- ✅ 1 angka (0-9)
- ✅ 1 karakter spesial (!@#$%^&* dll)
- ✅ Tidak mengandung common passwords
- ✅ Tidak mengandung sequential characters (123, abc)

**Testing:**
```json
// BAD - Akan ditolak
{
  "username": "john",
  "password": "password123"
}

// GOOD - Akan diterima
{
  "username": "john",
  "password": "MySecure@Pass123"
}
```

---

### 4. **JWT Refresh Tokens**

**Flow:**
1. Login → Dapat `accessToken` (60 min) & `refreshToken` (7 days)
2. Access token expired → Call `/api/auth/refresh` dengan refresh token
3. Dapat access token baru tanpa login ulang
4. Logout → Revoke refresh token

**Database Table: `refresh_tokens`**
- `id` INT PK
- `user_id` INT FK
- `token` VARCHAR(500) - The refresh token
- `expires_at` DATETIME
- `is_revoked` BOOLEAN
- `created_by_ip` VARCHAR(50)
- `created_by_device` VARCHAR(200)

**Endpoints:**
```bash
# Login
POST /api/auth/login
Response: { accessToken, refreshToken, ... }

# Refresh
POST /api/auth/refresh
Body: { "refreshToken": "..." }
Response: { accessToken }

# Logout
POST /api/auth/logout
Body: { "refreshToken": "..." }
```

---

### 5. **Secrets Management**

**BEFORE (❌ Tidak Aman):**
```json
// appsettings.json
{
  "Jwt": {
    "Key": "supersecretkey123456789" // ❌ HARDCODED!
  },
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;user=root;password=admin123" // ❌ EXPOSED!
  }
}
```

**AFTER (✅ Aman):**
```json
// appsettings.json
{
  "Jwt": {
    "Key": "" // Load from environment
  },
  "ConnectionStrings": {
    "DefaultConnection": "" // Load from environment
  }
}
```

```powershell
# Set via environment variables
$env:SIMA_JWT_SECRET = "your-secret-key"
$env:SIMA_DB_CONNECTION = "your-connection-string"
```

**Production Setup (Windows Server):**
```powershell
# Set as system environment variables (permanent)
[System.Environment]::SetEnvironmentVariable("SIMA_JWT_SECRET", "your-key", "Machine")
[System.Environment]::SetEnvironmentVariable("SIMA_DB_CONNECTION", "your-conn", "Machine")
```

---

### 6. **Security Headers**

**Headers yang ditambahkan:**

| Header | Value | Protection Against |
|--------|-------|-------------------|
| X-Content-Type-Options | nosniff | MIME type sniffing |
| X-Frame-Options | DENY | Clickjacking |
| X-XSS-Protection | 1; mode=block | XSS attacks |
| Strict-Transport-Security | max-age=31536000 | MITM attacks |
| Content-Security-Policy | default-src 'self' | XSS, injection |
| Referrer-Policy | strict-origin | Information leakage |
| Permissions-Policy | geolocation=() | Unauthorized access |

**Testing:**
```bash
# Check headers
curl -I https://localhost:7xxx/api/auth/test

# Should see:
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Strict-Transport-Security: max-age=31536000
...
```

---

## 🧪 TESTING SECURITY FEATURES

### Test 1: Password Strength Validation

```bash
# Test weak password
curl -X POST https://localhost:7xxx/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "weak123",
    "email": "test@example.com"
  }'

# Expected: 400 Bad Request
# Message: "Password minimal 8 karakter" atau "Password harus mengandung minimal 1 huruf besar"
```

### Test 2: Account Lockout

```bash
# Login with wrong password 5 times
for i in {1..5}; do
  curl -X POST https://localhost:7xxx/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{
      "emailOrPhone": "admin_bpjs",
      "password": "wrongpassword"
    }'
done

# 6th attempt should return 423 Locked
curl -X POST https://localhost:7xxx/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrPhone": "admin_bpjs",
    "password": "wrongpassword"
  }'
```

### Test 3: Rate Limiting

```bash
# Send 6 login requests rapidly
for i in {1..6}; do
  curl -X POST https://localhost:7xxx/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{
      "emailOrPhone": "test",
      "password": "test"
    }'
  sleep 0.5
done

# 6th request should return 429 Too Many Requests
```

### Test 4: Refresh Token Flow

```bash
# 1. Login
LOGIN_RESPONSE=$(curl -X POST https://localhost:7xxx/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrPhone": "admin_bpjs",
    "password": "Admin@BPJS2024!"
  }')

# Extract tokens
ACCESS_TOKEN=$(echo $LOGIN_RESPONSE | jq -r '.accessToken')
REFRESH_TOKEN=$(echo $LOGIN_RESPONSE | jq -r '.refreshToken')

# 2. Wait for access token to expire (60 minutes) or test immediately
# 3. Refresh access token
curl -X POST https://localhost:7xxx/api/auth/refresh \
  -H "Content-Type: application/json" \
  -d "{
    \"refreshToken\": \"$REFRESH_TOKEN\"
  }"

# Should return new accessToken
```

---

## 📊 DATABASE CHANGES

### New Columns in `users` table:

```sql
ALTER TABLE users 
ADD COLUMN failed_login_attempts INT DEFAULT 0,
ADD COLUMN lockout_end DATETIME NULL;
```

### New Table: `refresh_tokens`

```sql
CREATE TABLE refresh_tokens (
  id INT AUTO_INCREMENT PRIMARY KEY,
  user_id INT NOT NULL,
  token VARCHAR(500) NOT NULL UNIQUE,
  expires_at DATETIME NOT NULL,
  created_at DATETIME NOT NULL,
  is_revoked BOOLEAN DEFAULT FALSE,
  revoked_at DATETIME NULL,
  created_by_ip VARCHAR(50) NULL,
  created_by_device VARCHAR(200) NULL,
  FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
  INDEX idx_token (token),
  INDEX idx_user_revoked (user_id, is_revoked)
);
```

---

## 📝 LOGGING

**Log Files Location:** `logs/sima-bpjs-YYYYMMDD.log`

**Log Retention:** 30 days

**Log Events:**
- ✅ Application startup/shutdown
- ✅ User registration
- ✅ Successful logins
- ✅ Failed login attempts
- ✅ Account lockouts
- ✅ Token refresh
- ✅ HTTP requests (method, path, status, duration)
- ✅ Errors and exceptions

**Example Log:**
```
2024-01-15 10:30:45.123 [INF] User logged in successfully: admin_bpjs from IP: 192.168.1.100
2024-01-15 10:31:20.456 [WRN] Failed login attempt for: john@example.com from IP: 192.168.1.105
2024-01-15 10:32:10.789 [WRN] Account locked due to failed attempts: john@example.com from IP: 192.168.1.105
```

---

## 🎯 UNTUK PRESENTASI MATKUL

### Topics yang bisa dibahas:

1. **Authentication & Authorization**
   - JWT implementation
   - Role-Based Access Control (RBAC)
   - Token refresh mechanism

2. **Password Security**
   - PBKDF2 with SHA-256
   - Salting and key derivation
   - Timing attack prevention (constant-time comparison)
   - Password complexity requirements

3. **Attack Prevention**
   - Brute force protection (rate limiting + account lockout)
   - User enumeration prevention
   - CSRF protection concepts
   - XSS/Clickjacking via headers

4. **Audit & Monitoring**
   - Login activity tracking
   - Security event logging
   - IP address and device fingerprinting

5. **Secrets Management**
   - Environment variables vs hardcoded secrets
   - Secure configuration practices
   - Production deployment considerations

6. **OWASP Top 10 Coverage**
   - A01:2021 - Broken Access Control ✅
   - A02:2021 - Cryptographic Failures ✅
   - A03:2021 - Injection ✅ (via EF Core)
   - A04:2021 - Insecure Design ✅
   - A05:2021 - Security Misconfiguration ✅
   - A07:2021 - Identification and Authentication Failures ✅

---

## ⚠️ IMPORTANT NOTES

### Production Deployment:

1. **Environment Variables:**
   - Set `SIMA_JWT_SECRET` dengan key yang benar-benar random (min 32 chars)
   - Set `SIMA_DB_CONNECTION` dengan credentials production
   - Jangan gunakan default values

2. **Database:**
   - Backup database sebelum run migration
   - Test di development environment dulu
   - Run migration di production: `dotnet ef database update --connection "production-conn-string"`

3. **HTTPS:**
   - Gunakan SSL certificate yang valid
   - Enable HSTS (sudah otomatis jika HTTPS)

4. **Monitoring:**
   - Setup log monitoring (Splunk, ELK, dll)
   - Monitor rate limit hits
   - Monitor lockout events
   - Setup alerts untuk suspicious activities

5. **Rate Limit Tuning:**
   - Adjust limits based on real usage patterns
   - Consider different limits for different user roles
   - Monitor false positives

---

## 🔄 ROLLBACK PLAN

Jika ada masalah, rollback ke versi sebelumnya:

```powershell
# 1. Stop API
# 2. Restore backup files
cd sima_Bpjs_api
Remove-Item controllers/AuthController.cs
move controllers/AuthController_OLD_BACKUP.cs controllers/AuthController.cs
Remove-Item Program.cs
move Program_OLD_BACKUP.cs Program.cs

# 3. Rollback database migration
dotnet ef database update PreviousMigrationName

# 4. Restart API
dotnet run
```

---

## 📞 SUPPORT

Jika ada masalah atau pertanyaan:
1. Check logs di `logs/sima-bpjs-*.log`
2. Verify environment variables: `echo $env:SIMA_JWT_SECRET`
3. Check database migration status: `dotnet ef migrations list`
4. Review error messages dalam console

---

## ✅ CHECKLIST IMPLEMENTASI

- [ ] Install packages (`dotnet restore`)
- [ ] Create migration (`dotnet ef migrations add AddSecurityEnhancements`)
- [ ] Update database (`dotnet ef database update`)
- [ ] Set environment variables (`.\setup-environment.ps1`)
- [ ] Test API startup (`dotnet run`)
- [ ] Test password validation
- [ ] Test account lockout
- [ ] Test rate limiting
- [ ] Test refresh token flow
- [ ] Check logs are being created
- [ ] Test security headers
- [ ] Update frontend to use refresh tokens
- [ ] Document admin password (Default: `Admin@BPJS2024!`)

---

## 🎓 LEARNING RESOURCES

- OWASP Top 10: https://owasp.org/Top10/
- JWT Best Practices: https://tools.ietf.org/html/rfc8725
- Password Storage Cheat Sheet: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html
- Rate Limiting Patterns: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly

---

**Version:** 2.0 - Enhanced Security  
**Last Updated:** 2024  
**Author:** Security Enhancement Team

