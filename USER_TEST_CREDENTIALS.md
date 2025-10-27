# 👤 USER TEST CREDENTIALS - SIMA BPJS API

## 🎯 Overview

Contoh data lengkap untuk testing API dengan **role USER** (bukan admin).

---

## 🚀 **QUICK START - LOGIN USER**

### **Default User Account (Sudah Ada di Database)**

Jika Anda sudah register user sebelumnya, gunakan credentials yang sudah dibuat.

**Login Request:**
```json
POST /api/auth/login
Content-Type: application/json

{
  "emailOrPhone": "user@example.com",
  "password": "User@Pass2024!"
}
```

---

## 📝 **CONTOH DATA REGISTER USER BARU**

### **User 1: John Doe (Recommended untuk Testing)**

```json
POST /api/auth/register
Content-Type: application/json

{
  "username": "john_doe",
  "password": "SecurePass@2024!",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "dateOfBirth": "1995-05-15",
  "nik": "3201011505950001"
}
```

**Setelah Register, Login dengan:**
```json
POST /api/auth/login

{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

**Atau login dengan Phone Number:**
```json
{
  "emailOrPhone": "081234567890",
  "password": "SecurePass@2024!"
}
```

**Atau login dengan Username:**
```json
{
  "emailOrPhone": "john_doe",
  "password": "SecurePass@2024!"
}
```

---

### **User 2: Jane Smith**

```json
POST /api/auth/register

{
  "username": "jane_smith",
  "password": "MyStrong@Pass123",
  "email": "jane.smith@example.com",
  "phoneNumber": "082345678901",
  "dateOfBirth": "1998-08-20",
  "nik": "3201012008980002"
}
```

**Login:**
```json
POST /api/auth/login

{
  "emailOrPhone": "jane.smith@example.com",
  "password": "MyStrong@Pass123"
}
```

---

### **User 3: Ahmad Budiman**

```json
POST /api/auth/register

{
  "username": "ahmad_budiman",
  "password": "Ahmad@Bpjs2024!",
  "email": "ahmad.budiman@email.com",
  "phoneNumber": "083456789012",
  "dateOfBirth": "1990-12-10",
  "nik": "3201011012900003"
}
```

**Login:**
```json
POST /api/auth/login

{
  "emailOrPhone": "083456789012",
  "password": "Ahmad@Bpjs2024!"
}
```

---

### **User 4: Siti Nurhaliza**

```json
POST /api/auth/register

{
  "username": "siti_nurhaliza",
  "password": "Siti@Secure2024",
  "email": "siti.nurhaliza@gmail.com",
  "phoneNumber": "084567890123",
  "dateOfBirth": "1992-03-25",
  "nik": "3201012503920004"
}
```

**Login:**
```json
POST /api/auth/login

{
  "emailOrPhone": "siti.nurhaliza@gmail.com",
  "password": "Siti@Secure2024"
}
```

---

### **User 5: Budi Santoso**

```json
POST /api/auth/register

{
  "username": "budi_santoso",
  "password": "Budi@Pass2024!",
  "email": "budi.santoso@yahoo.com",
  "phoneNumber": "085678901234",
  "dateOfBirth": "1988-07-08",
  "nik": "3201010807880005"
}
```

**Login:**
```json
POST /api/auth/login

{
  "emailOrPhone": "budi_santoso",
  "password": "Budi@Pass2024!"
}
```

---

## 🔐 **PASSWORD REQUIREMENTS**

Semua password harus memenuhi:
- ✅ Minimal 8 karakter
- ✅ 1 huruf besar (A-Z)
- ✅ 1 huruf kecil (a-z)
- ✅ 1 angka (0-9)
- ✅ 1 karakter spesial (!@#$%^&*)

**Contoh Password VALID:**
- ✅ `SecurePass@2024!`
- ✅ `MyStrong@Pass123`
- ✅ `Ahmad@Bpjs2024!`
- ✅ `User@Password2024`

**Contoh Password INVALID:**
- ❌ `password` (no uppercase, no number, no special)
- ❌ `Password` (no number, no special)
- ❌ `Password123` (no special char)
- ❌ `Pass@1` (too short)

---

## 📱 **LOGIN OPTIONS**

User bisa login dengan salah satu dari:

### **1. Email**
```json
{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

### **2. Phone Number**
```json
{
  "emailOrPhone": "081234567890",
  "password": "SecurePass@2024!"
}
```

### **3. Username**
```json
{
  "emailOrPhone": "john_doe",
  "password": "SecurePass@2024!"
}
```

---

## 🎯 **COMPLETE TESTING FLOW**

### **Step 1: Register User Baru**

```http
POST http://localhost:7XXX/api/auth/register
Content-Type: application/json

{
  "username": "test_user",
  "password": "TestUser@2024!",
  "email": "test.user@example.com",
  "phoneNumber": "081234567999",
  "dateOfBirth": "2000-01-01",
  "nik": "3201010101000099"
}
```

**Expected Response:**
```json
{
  "id": 5,
  "username": "test_user",
  "role": "USER",
  "nik": "3201010101000099",
  "email": "test.user@example.com",
  "phoneNumber": "081234567999",
  "dateOfBirth": "2000-01-01T00:00:00"
}
```

---

### **Step 2: Login dengan User yang Baru Dibuat**

```http
POST http://localhost:7XXX/api/auth/login
Content-Type: application/json

{
  "emailOrPhone": "test.user@example.com",
  "password": "TestUser@2024!"
}
```

**Expected Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "A7x8K2mP9qR3vB5nC1wD4eF6gH8jK0lM...",
  "username": "test_user",
  "role": "USER",
  "email": "test.user@example.com",
  "phoneNumber": "081234567999",
  "expiresIn": 3600
}
```

---

### **Step 3: Gunakan Token untuk Request Lain**

**COPY `accessToken` dari response login, lalu gunakan di header:**

```http
GET http://localhost:7XXX/api/bpjs
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 🧪 **TESTING SCENARIOS**

### **Scenario 1: Login dengan Email**

```json
// Register
{
  "username": "scenario1_user",
  "password": "Scenario@1Pass!",
  "email": "scenario1@test.com",
  "phoneNumber": "081111111111",
  "nik": "3201010101000111"
}

// Login
{
  "emailOrPhone": "scenario1@test.com",
  "password": "Scenario@1Pass!"
}
```

---

### **Scenario 2: Login dengan Phone Number**

```json
// Register
{
  "username": "scenario2_user",
  "password": "Scenario@2Pass!",
  "email": "scenario2@test.com",
  "phoneNumber": "082222222222",
  "nik": "3201010101000222"
}

// Login
{
  "emailOrPhone": "082222222222",
  "password": "Scenario@2Pass!"
}
```

---

### **Scenario 3: Login dengan Username**

```json
// Register
{
  "username": "scenario3_user",
  "password": "Scenario@3Pass!",
  "email": "scenario3@test.com",
  "phoneNumber": "083333333333",
  "nik": "3201010101000333"
}

// Login
{
  "emailOrPhone": "scenario3_user",
  "password": "Scenario@3Pass!"
}
```

---

## ❌ **ERROR SCENARIOS untuk Testing**

### **1. Test Password Lemah**

```json
POST /api/auth/register

{
  "username": "weak_pass_user",
  "password": "weak",
  "email": "weak@test.com",
  "phoneNumber": "081999999991"
}
```

**Expected Error:**
```json
{
  "message": "Password minimal 8 karakter"
}
```

---

### **2. Test Login dengan Password Salah**

```json
// Register user dulu
{
  "username": "test_wrong_pass",
  "password": "Correct@Pass2024",
  "email": "wrongpass@test.com",
  "phoneNumber": "081999999992",
  "nik": "3201010101000992"
}

// Login dengan password salah (5x untuk test account lockout)
{
  "emailOrPhone": "wrongpass@test.com",
  "password": "WrongPassword123!"
}
```

**After 5 failed attempts:**
```json
{
  "message": "Akun dikunci selama 15 menit karena terlalu banyak percobaan login gagal.",
  "lockoutEnd": "2024-10-26T10:45:00Z"
}
```

---

### **3. Test Duplicate Username**

```json
// Register user pertama
{
  "username": "duplicate_user",
  "password": "First@User2024",
  "email": "first@test.com",
  "phoneNumber": "081999999993",
  "nik": "3201010101000993"
}

// Register dengan username yang sama
{
  "username": "duplicate_user",
  "password": "Second@User2024",
  "email": "second@test.com",
  "phoneNumber": "081999999994",
  "nik": "3201010101000994"
}
```

**Expected Error:**
```json
{
  "message": "Username sudah digunakan"
}
```

---

## 📋 **POSTMAN/SWAGGER USAGE**

### **Untuk Swagger UI:**

1. Buka: `http://localhost:7XXX/swagger`
2. Expand: `POST /api/auth/register`
3. Click: "Try it out"
4. Paste salah satu contoh data di atas
5. Click: "Execute"
6. Copy `accessToken` dari response
7. Click: "Authorize" button (🔓)
8. Paste: `Bearer {accessToken}`
9. Click: "Authorize"
10. Sekarang bisa test endpoint lain!

### **Untuk Postman:**

1. Create request: `POST {{base_url}}/api/auth/register`
2. Body → raw → JSON
3. Paste contoh data register
4. Send
5. Save `accessToken` ke environment variable
6. Add header ke request lain: `Authorization: Bearer {{accessToken}}`

---

## 🔄 **REFRESH TOKEN FLOW**

### **1. Login (Get Tokens)**

```json
POST /api/auth/login

{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

**Response:**
```json
{
  "accessToken": "short.lived.token...",
  "refreshToken": "long.lived.token...",
  "username": "john_doe",
  "role": "USER"
}
```

### **2. Access Token Expired? Refresh It!**

```json
POST /api/auth/refresh

{
  "refreshToken": "long.lived.token..."
}
```

**Response:**
```json
{
  "accessToken": "new.short.lived.token...",
  "expiresIn": 3600,
  "message": "Token berhasil diperbarui"
}
```

### **3. Logout (Revoke Refresh Token)**

```json
POST /api/auth/logout
Authorization: Bearer {accessToken}

{
  "refreshToken": "long.lived.token..."
}
```

---

## 📊 **SUMMARY TABLE**

| User            | Email                      | Phone        | Password            | NIK              |
|-----------------|----------------------------|--------------|---------------------|------------------|
| john_doe        | john.doe@example.com       | 081234567890 | SecurePass@2024!    | 3201011505950001 |
| jane_smith      | jane.smith@example.com     | 082345678901 | MyStrong@Pass123    | 3201012008980002 |
| ahmad_budiman   | ahmad.budiman@email.com    | 083456789012 | Ahmad@Bpjs2024!     | 3201011012900003 |
| siti_nurhaliza  | siti.nurhaliza@gmail.com   | 084567890123 | Siti@Secure2024     | 3201012503920004 |
| budi_santoso    | budi.santoso@yahoo.com     | 085678901234 | Budi@Pass2024!      | 3201010807880005 |

---

## 🎯 **NEXT STEPS AFTER LOGIN**

Setelah login sebagai USER, Anda bisa test:

### **✅ Endpoints yang Bisa Diakses USER:**

1. **KTP/KK Management**
   - `GET /api/ktpkk` - List all
   - `POST /api/ktpkk` - Create
   - `GET /api/ktpkk/{nik}` - Get by NIK
   - `PUT /api/ktpkk/{nik}` - Update
   - `DELETE /api/ktpkk/{nik}` - Delete

2. **BPJS Management**
   - `GET /api/bpjs` - List all
   - `POST /api/bpjs` - Create
   - `GET /api/bpjs/{id}` - Get by ID
   - `PUT /api/bpjs/{id}` - Update
   - `DELETE /api/bpjs/{id}` - Delete

3. **Pembayaran Management**
   - `GET /api/pembayaran` - List all
   - `POST /api/pembayaran` - Create
   - `GET /api/pembayaran/{id}` - Get by ID

4. **Login Activity**
   - `GET /api/loginactivity/my-activities` - View own login history

### **❌ Endpoints yang TIDAK Bisa Diakses (Admin Only):**

- `POST /api/bpjs/{id}/approve` - Approve BPJS (403 Forbidden)
- `POST /api/bpjs/{id}/deactivate` - Deactivate BPJS (403 Forbidden)
- `GET /api/loginactivity` - View all login activities (403 Forbidden)

---

## 🚨 **COMMON ERRORS & SOLUTIONS**

### **Error 400: "Password minimal 8 karakter"**
**Solution:** Gunakan password yang memenuhi requirements (min 8 chars, uppercase, lowercase, number, special)

### **Error 401: "Kredensial tidak valid"**
**Solution:** Cek username/email/phone dan password. Pastikan sudah register dulu.

### **Error 409: "Username sudah digunakan"**
**Solution:** Gunakan username yang berbeda.

### **Error 423: "Akun terkunci"**
**Solution:** Tunggu 15 menit, atau gunakan user lain untuk testing.

---

## 💡 **TIPS TESTING**

1. **Gunakan user berbeda** untuk setiap test scenario
2. **Save tokens** di environment variables (Postman) atau variable (Swagger)
3. **Test error scenarios** untuk memastikan validation works
4. **Test refresh token** setelah access token expire
5. **Test logout** untuk memastikan token revocation works

---

## 📞 **SUPPORT**

Jika mengalami masalah:
1. Check API logs: `logs/sima-bpjs-*.log`
2. Verify database: User sudah ter-create?
3. Check token: Masih valid? (60 menit)
4. Test dengan Swagger UI dulu sebelum Postman

---

**Happy Testing! 🎉**

_Dokumentasi dibuat untuk memudahkan testing dengan role USER_

