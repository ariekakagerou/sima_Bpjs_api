# 🔧 SWAGGER & POSTMAN TESTING GUIDE

## 📋 Complete Guide untuk Testing USER dengan Swagger UI dan Postman

---

## 🎨 **OPTION 1: SWAGGER UI** (Paling Mudah!)

### **Step 1: Jalankan API**

```powershell
cd c:/Users/LENOVO/BPJS(sima)/sima_Bpjs_api
dotnet run
```

**Output yang diharapkan:**
```
Now listening on: http://localhost:7189
Now listening on: https://localhost:7190
Application started. Press Ctrl+C to shut down.
```

**Note:** Port bisa berbeda (7189, 5189, dll). Lihat di console output!

---

### **Step 2: Buka Swagger UI**

Buka browser dan akses salah satu URL:
- **HTTP:** `http://localhost:7189/swagger`
- **HTTPS:** `https://localhost:7190/swagger`

*(Sesuaikan port dengan yang muncul di console)*

---

### **Step 3: Register User Baru** 📝

#### **3.1 Expand Endpoint Register**

1. Scroll ke section **Auth**
2. Click: `POST /api/Auth/register` (warna hijau)
3. Click button: **"Try it out"** (kanan atas)

#### **3.2 Input Data User**

Hapus contoh default dan paste salah satu data ini:

**RECOMMENDED - John Doe:**
```json
{
  "username": "john_doe",
  "password": "SecurePass@2024!",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "dateOfBirth": "1995-05-15",
  "nik": "3201011505950001"
}
```

**Alternatif - Test User:**
```json
{
  "username": "test_user",
  "password": "TestUser@2024!",
  "email": "test.user@example.com",
  "phoneNumber": "081234567999",
  "dateOfBirth": "2000-01-01",
  "nik": "3201010101000099"
}
```

#### **3.3 Execute Request**

1. Click button: **"Execute"** (biru besar)
2. Scroll down ke **"Responses"**
3. Lihat **Code: 201** (success)

**Response Example:**
```json
{
  "id": 5,
  "username": "john_doe",
  "role": "USER",
  "nik": "3201011505950001",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "dateOfBirth": "1995-05-15T00:00:00"
}
```

✅ **User berhasil dibuat!**

---

### **Step 4: Login User** 🔐

#### **4.1 Expand Endpoint Login**

1. Scroll ke: `POST /api/Auth/login`
2. Click untuk expand
3. Click: **"Try it out"**

#### **4.2 Input Login Credentials**

Paste data login (pilih salah satu method):

**Login dengan Email:**
```json
{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

**Login dengan Phone:**
```json
{
  "emailOrPhone": "081234567890",
  "password": "SecurePass@2024!"
}
```

**Login dengan Username:**
```json
{
  "emailOrPhone": "john_doe",
  "password": "SecurePass@2024!"
}
```

#### **4.3 Execute & Copy Token**

1. Click: **"Execute"**
2. Lihat Response:

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obl9kb2UiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVU0VSIiwianRpIjoiYTFiMmMzZDQtZTVmNi03ODkwLTEyMzQtNTY3ODkwYWJjZGVmIiwiZXhwIjoxNzI5OTQwNDAwLCJpc3MiOiJzaW1hX2JwanNfYXBpIiwiYXVkIjoic2ltYV9icGpzX2FwaV91c2VycyJ9.ABC123...",
  "refreshToken": "K9mP2qR5vB8nC1wD4eF7gH0jL3oM6pN9rS2tU5xY8zA...",
  "username": "john_doe",
  "role": "USER",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "expiresIn": 3600
}
```

3. **COPY** semua text dari `accessToken` (dari `eyJ...` sampai akhir)

---

### **Step 5: Authorize di Swagger** 🔓

#### **5.1 Click Tombol Authorize**

1. Scroll ke paling atas Swagger page
2. Click button: **"Authorize"** 🔓 (kanan atas, sebelah logo)
3. Akan muncul popup "Available authorizations"

#### **5.2 Input Token**

1. Di field **"Value"**, ketik:
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```
   ⚠️ **PENTING:** Harus ada kata `Bearer ` (dengan spasi) sebelum token!

2. Click: **"Authorize"** (button biru)
3. Click: **"Close"**

✅ **Sekarang Anda sudah ter-authorize!**

Icon 🔓 akan berubah jadi 🔒

---

### **Step 6: Test Endpoint yang Memerlukan Auth** ✅

Sekarang Anda bisa test semua endpoint yang butuh authorization:

#### **6.1 Get All BPJS**

1. Expand: `GET /api/Bpjs`
2. Click: "Try it out"
3. Click: "Execute"
4. Lihat response data BPJS

#### **6.2 Get All KTP/KK**

1. Expand: `GET /api/KtpKk`
2. Click: "Try it out"
3. Click: "Execute"
4. Lihat response data KTP/KK

#### **6.3 Get My Login Activities**

1. Expand: `GET /api/LoginActivity/my-activities`
2. Click: "Try it out"
3. Optional: Set `limit` parameter (default 10)
4. Click: "Execute"
5. Lihat riwayat login Anda!

**Response Example:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "username": "john_doe",
      "ipAddress": "::1",
      "device": "Windows 10",
      "browser": "Chrome",
      "status": "BERHASIL",
      "loginTime": "2024-10-26T10:30:45"
    }
  ],
  "message": "Login activities retrieved successfully"
}
```

---

### **Step 7: Test Refresh Token** 🔄

#### **Ketika Access Token Sudah Expire (60 menit):**

1. Expand: `POST /api/Auth/refresh`
2. Click: "Try it out"
3. Paste refresh token yang Anda save dari login:
   ```json
   {
     "refreshToken": "K9mP2qR5vB8nC1wD4eF7gH0jL3oM6pN9rS2tU5xY8zA..."
   }
   ```
4. Click: "Execute"
5. Copy **NEW** `accessToken` dari response
6. Update authorization (Step 5) dengan token baru

---

### **Step 8: Test Error Scenarios** ❌

#### **8.1 Test Weak Password**

1. Expand: `POST /api/Auth/register`
2. Try it out
3. Input:
   ```json
   {
     "username": "weak_test",
     "password": "weak",
     "email": "weak@test.com",
     "phoneNumber": "081999999991"
   }
   ```
4. Execute
5. **Expected Error 400:**
   ```json
   {
     "message": "Password minimal 8 karakter"
   }
   ```

#### **8.2 Test Wrong Password (Account Lockout)**

1. Expand: `POST /api/Auth/login`
2. Try it out
3. Input wrong password 5 kali:
   ```json
   {
     "emailOrPhone": "john.doe@example.com",
     "password": "WrongPassword123!"
   }
   ```
4. Execute 5 kali
5. **Expected Error 423 on 5th attempt:**
   ```json
   {
     "message": "Akun dikunci selama 15 menit karena terlalu banyak percobaan login gagal.",
     "lockoutEnd": "2024-10-26T10:45:00Z"
   }
   ```

#### **8.3 Test Admin-Only Endpoint (Should Fail)**

1. Expand: `POST /api/Bpjs/{id}/approve`
2. Try it out
3. Input id: `1`
4. Execute
5. **Expected Error 403:**
   ```json
   {
     "message": "Forbidden"
   }
   ```

---

## 📬 **OPTION 2: POSTMAN**

### **Step 1: Download & Install Postman**

Download dari: https://www.postman.com/downloads/

---

### **Step 2: Create New Collection**

1. Open Postman
2. Click: **"New"** → **"Collection"**
3. Name: `SIMA BPJS API - User Testing`
4. Click: **"Create"**

---

### **Step 3: Setup Environment Variables**

#### **3.1 Create Environment**

1. Click: ⚙️ **"Environments"** (sidebar kiri)
2. Click: **"+"** untuk create environment baru
3. Name: `SIMA BPJS Local`

#### **3.2 Add Variables**

Add variables berikut:

| Variable        | Initial Value                | Current Value                |
|-----------------|------------------------------|------------------------------|
| `base_url`      | `http://localhost:7189`      | `http://localhost:7189`      |
| `access_token`  |                              |                              |
| `refresh_token` |                              |                              |

*(Kosongkan `access_token` dan `refresh_token`, akan di-fill otomatis)*

4. Click: **"Save"**
5. Select environment: `SIMA BPJS Local` (dropdown kanan atas)

---

### **Step 4: Create Register Request**

#### **4.1 Add Request**

1. Click collection: `SIMA BPJS API - User Testing`
2. Click: **"Add request"**
3. Name: `Register User`

#### **4.2 Configure Request**

- **Method:** POST
- **URL:** `{{base_url}}/api/auth/register`
- **Headers:**
  - Key: `Content-Type`
  - Value: `application/json`

#### **4.3 Request Body**

Click tab: **"Body"** → Select: **"raw"** → Type: **"JSON"**

Paste:
```json
{
  "username": "john_doe",
  "password": "SecurePass@2024!",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "dateOfBirth": "1995-05-15",
  "nik": "3201011505950001"
}
```

#### **4.4 Send Request**

1. Click: **"Send"**
2. Lihat response di bawah (Status: 201 Created)
3. Click: **"Save"**

---

### **Step 5: Create Login Request (dengan Auto-Save Token)**

#### **5.1 Add Request**

1. Add request: `Login User`
2. Method: **POST**
3. URL: `{{base_url}}/api/auth/login`

#### **5.2 Headers**

- `Content-Type`: `application/json`

#### **5.3 Body**

```json
{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

#### **5.4 Auto-Save Token to Environment** 🎯

Click tab: **"Tests"** (setelah Body)

Paste script ini:
```javascript
// Auto-save tokens to environment variables
if (pm.response.code === 200) {
    const response = pm.response.json();
    
    // Save access token
    pm.environment.set("access_token", response.accessToken);
    
    // Save refresh token
    pm.environment.set("refresh_token", response.refreshToken);
    
    console.log("✅ Tokens saved to environment!");
    console.log("Access Token:", response.accessToken.substring(0, 50) + "...");
    console.log("Refresh Token:", response.refreshToken.substring(0, 50) + "...");
}
```

#### **5.5 Send & Save**

1. Click: **"Send"**
2. Check **"Console"** (bottom left) - tokens should be saved!
3. Check **"Environment quick look"** 👁️ (kanan atas) - tokens ada!
4. Click: **"Save"**

---

### **Step 6: Create Authenticated Request**

#### **6.1 Add Request: Get All BPJS**

1. Add request: `Get All BPJS`
2. Method: **GET**
3. URL: `{{base_url}}/api/bpjs`

#### **6.2 Authorization** 🔐

Click tab: **"Authorization"**

- **Type:** `Bearer Token`
- **Token:** `{{access_token}}`

*(Ini akan otomatis ambil dari environment variable)*

#### **6.3 Send Request**

1. Click: **"Send"**
2. Lihat data BPJS di response
3. Click: **"Save"**

---

### **Step 7: Create More Requests**

Duplicate request dan ubah:

#### **Get All KTP/KK**
- Method: GET
- URL: `{{base_url}}/api/ktpkk`
- Auth: Bearer Token `{{access_token}}`

#### **Get My Login Activities**
- Method: GET
- URL: `{{base_url}}/api/loginactivity/my-activities?limit=10`
- Auth: Bearer Token `{{access_token}}`

#### **Refresh Token**
- Method: POST
- URL: `{{base_url}}/api/auth/refresh`
- Body:
  ```json
  {
    "refreshToken": "{{refresh_token}}"
  }
  ```
- Tests (auto-update access token):
  ```javascript
  if (pm.response.code === 200) {
      const response = pm.response.json();
      pm.environment.set("access_token", response.accessToken);
      console.log("✅ Access token refreshed!");
  }
  ```

#### **Logout**
- Method: POST
- URL: `{{base_url}}/api/auth/logout`
- Auth: Bearer Token `{{access_token}}`
- Body:
  ```json
  {
    "refreshToken": "{{refresh_token}}"
  }
  ```

---

### **Step 8: Test Complete Flow**

**Testing Order:**
1. ▶️ Register User
2. ▶️ Login User (tokens auto-saved!)
3. ▶️ Get All BPJS (menggunakan token)
4. ▶️ Get All KTP/KK
5. ▶️ Get My Login Activities
6. ⏰ *Wait 60 min or test immediately*
7. ▶️ Refresh Token (new access token)
8. ▶️ Logout (revoke tokens)

---

## 🎯 **COMPARISON: Swagger vs Postman**

| Feature                | Swagger UI                      | Postman                        |
|------------------------|---------------------------------|--------------------------------|
| **Setup**              | ✅ Zero setup (built-in)        | ⚠️ Need download & setup       |
| **Documentation**      | ✅ Auto-generated from code     | ⚠️ Manual                      |
| **Token Management**   | ⚠️ Manual copy-paste            | ✅ Auto-save with scripts      |
| **Environment Vars**   | ❌ Not supported                | ✅ Fully supported             |
| **Collections**        | ❌ Not supported                | ✅ Save & organize requests    |
| **Testing Scripts**    | ❌ Not supported                | ✅ Pre/Post request scripts    |
| **Team Collaboration** | ❌ Not supported                | ✅ Share collections           |
| **Offline Work**       | ❌ Need API running             | ✅ Can prepare offline         |
| **Learning Curve**     | ✅ Easy                         | ⚠️ Moderate                    |

**Recommendation:**
- 🎨 **Swagger:** Quick testing, documentation, exploration
- 📬 **Postman:** Complex workflows, automation, team work

---

## 💡 **PRO TIPS**

### **For Swagger:**

1. **Keep Console Open:** F12 → Console tab untuk lihat errors
2. **Save Token:** Copy token ke notepad sementara
3. **Test Incrementally:** Register → Login → Single endpoint → Multiple endpoints
4. **Use Examples:** Swagger punya example values, edit aja
5. **Check Response Schema:** Lihat structure data yang dikembalikan

### **For Postman:**

1. **Use Environment Variables:** Jangan hardcode URL/tokens
2. **Use Tests Scripts:** Auto-save tokens, validate responses
3. **Organize Collections:** Group by feature (Auth, BPJS, etc)
4. **Use Pre-request Scripts:** Auto-refresh expired tokens
5. **Export Collection:** Share dengan tim atau backup
6. **Use Examples:** Save response examples untuk documentation

---

## 🔄 **WORKFLOW RECOMMENDATION**

### **Development Phase:**
```
Swagger UI → Quick manual testing
     ↓
Works? → Create in Postman for automation
```

### **Testing Phase:**
```
Postman Collections → Organized test scenarios
     ↓
Run all → Validate all endpoints
```

### **Documentation Phase:**
```
Swagger UI → Auto-generated API docs
     ↓
Export → Share with frontend team
```

---

## 🚨 **TROUBLESHOOTING**

### **Swagger Issues:**

**Problem:** "Failed to fetch"
- ✅ Check API is running (`dotnet run`)
- ✅ Check URL and port correct
- ✅ Try HTTPS instead of HTTP

**Problem:** 401 Unauthorized
- ✅ Re-login and get new token
- ✅ Check token format: `Bearer {token}` (with space!)
- ✅ Token might be expired (60 min)

**Problem:** Can't see my requests
- ✅ Swagger doesn't save requests, use browser bookmarks

### **Postman Issues:**

**Problem:** Variable not working
- ✅ Check environment is selected (dropdown kanan atas)
- ✅ Check variable name spelling
- ✅ Use `{{variable_name}}` format

**Problem:** Token not auto-saved
- ✅ Check Tests script syntax
- ✅ Check response is 200/201
- ✅ Open Console (bottom left) untuk debug

**Problem:** CORS error
- ✅ CORS is browser issue, Postman doesn't have this
- ✅ If testing in browser, make sure API CORS config correct

---

## 📊 **QUICK REFERENCE**

### **User Credentials (Copy-Paste Ready):**

```json
// Register
{
  "username": "john_doe",
  "password": "SecurePass@2024!",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "dateOfBirth": "1995-05-15",
  "nik": "3201011505950001"
}

// Login
{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

### **Postman Test Script (Auto-Save Token):**

```javascript
if (pm.response.code === 200) {
    const response = pm.response.json();
    pm.environment.set("access_token", response.accessToken);
    pm.environment.set("refresh_token", response.refreshToken);
    console.log("✅ Tokens saved!");
}
```

---

## 📞 **NEED HELP?**

**Check:**
1. API logs: `logs/sima-bpjs-*.log`
2. Console output (Swagger: F12, Postman: Console tab)
3. Documentation: `USER_TEST_CREDENTIALS.md`
4. Quick reference: `USER_QUICK_REFERENCE.md`

---

**Happy Testing! 🎉**

_Complete guide untuk Swagger UI & Postman testing_

