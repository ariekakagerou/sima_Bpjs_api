# 🎨 SWAGGER UI QUICK START GUIDE

## ⚡ Testing USER Role dalam 5 Menit dengan Swagger!

Zero setup, built-in documentation, instant testing! 🚀

---

## 🚀 **STEP 1: Start API (30 detik)**

```powershell
cd c:/Users/LENOVO/BPJS(sima)/sima_Bpjs_api
dotnet run
```

**Wait for:**
```
✅ Now listening on: http://localhost:7189
✅ Now listening on: https://localhost:7190
```

**Note:** Port number might be different! Lihat di console output.

---

## 🌐 **STEP 2: Open Swagger UI (10 detik)**

Open browser, go to:

**HTTP:**
```
http://localhost:7189/swagger
```

**HTTPS:**
```
https://localhost:7190/swagger
```

*(Use port from your console output!)*

**You'll see:** Beautiful API documentation page! 📄

---

## 📝 **STEP 3: Register User (60 detik)**

### **3.1 Find Register Endpoint**

Scroll ke section: **Auth** (warna hijau)

Click to expand:
```
POST /api/Auth/register
```

### **3.2 Try It Out**

Click button: **"Try it out"** (kanan atas request section)

### **3.3 Input User Data**

**Hapus** contoh default, **paste** data ini:

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

**Why this data?**
- ✅ Password memenuhi syarat kekuatan (8+ chars, uppercase, lowercase, digit, special)
- ✅ NIK format valid (16 digit)
- ✅ Email valid
- ✅ Phone valid

### **3.4 Execute**

Click: **"Execute"** (button biru besar)

**Wait 1-2 seconds...**

### **3.5 Check Response**

Scroll down ke **"Responses"**

**Success Response:**
```
Code: 201
```

```json
{
  "id": 5,
  "username": "john_doe",
  "role": "USER",
  "nik": "3201011505950001",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "dateOfBirth": "1995-05-15T00:00:00",
  "createdAt": "2024-10-26T10:30:00"
}
```

✅ **User registered successfully!**

---

## 🔐 **STEP 4: Login & Get Token (60 detik)**

### **4.1 Find Login Endpoint**

Scroll ke:
```
POST /api/Auth/login
```

Click to expand → Click **"Try it out"**

### **4.2 Input Login Credentials**

Paste:

```json
{
  "emailOrPhone": "john.doe@example.com",
  "password": "SecurePass@2024!"
}
```

**Alternative login methods:**

**By Phone:**
```json
{
  "emailOrPhone": "081234567890",
  "password": "SecurePass@2024!"
}
```

**By Username:**
```json
{
  "emailOrPhone": "john_doe",
  "password": "SecurePass@2024!"
}
```

### **4.3 Execute**

Click: **"Execute"**

### **4.4 COPY TOKEN** 📋

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obl9kb2UiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVU0VSIiwianRpIjoiYTFiMmMzZDQtZTVmNi03ODkwLTEyMzQtNTY3ODkwYWJjZGVmIiwiZXhwIjoxNzI5OTQwNDAwLCJpc3MiOiJzaW1hX2JwanNfYXBpIiwiYXVkIjoic2ltYV9icGpzX2FwaV91c2VycyJ9.ABC123DEF456...",
  "refreshToken": "K9mP2qR5vB8nC1wD4eF7gH0jL3oM6pN9rS2tU5xY8zA1bC2dE3fG4hI5jK6lM7nO8pQ9r",
  "username": "john_doe",
  "role": "USER",
  "email": "john.doe@example.com",
  "phoneNumber": "081234567890",
  "expiresIn": 3600
}
```

**IMPORTANT:** 
1. **COPY** semua text dari `accessToken` (mulai dari `eyJ...` sampai akhir)
2. Paste ke Notepad/text editor sementara
3. Juga **COPY** `refreshToken` (untuk nanti)

---

## 🔓 **STEP 5: Authorize Swagger (30 detik)**

### **5.1 Click Authorize Button**

Scroll ke **paling atas** halaman Swagger

Click button: 🔓 **"Authorize"** (kanan atas, sebelah logo)

**Popup muncul:** "Available authorizations"

### **5.2 Input Token**

Di field **"Value"**, ketik:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

⚠️ **CRITICAL:** 
- Harus ada kata `Bearer ` (dengan **spasi**) sebelum token!
- Format: `Bearer <paste_token_disini>`

**Example:**
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiam9obl9kb2UiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVU0VSIiwianRpIjoiYTFiMmMzZDQtZTVmNi03ODkwLTEyMzQtNTY3ODkwYWJjZGVmIiwiZXhwIjoxNzI5OTQwNDAwLCJpc3MiOiJzaW1hX2JwanNfYXBpIiwiYXVkIjoic2ltYV9icGpzX2FwaV91c2VycyJ9.ABC123DEF456...
```

### **5.3 Authorize & Close**

1. Click: **"Authorize"** (button biru)
2. Message muncul: "Authorized"
3. Click: **"Close"**

✅ **You're now authorized!**

Icon berubah: 🔓 → 🔒

---

## ✅ **STEP 6: Test Protected Endpoints (60 detik)**

Now you can access endpoints that require authentication!

### **6.1 Get All BPJS Data**

1. Scroll ke section: **Bpjs**
2. Expand: `GET /api/Bpjs`
3. Click: **"Try it out"**
4. Click: **"Execute"**

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "nikBpjs": "0001234567890",
      "nomorBpjs": "0001234567890123",
      "nik": "3201010101010001",
      // ... more data
    }
  ],
  "message": "Success"
}
```

✅ **Data retrieved successfully!**

### **6.2 Get All KTP/KK Data**

1. Scroll ke section: **KtpKk**
2. Expand: `GET /api/KtpKk`
3. Click: **"Try it out"**
4. Click: **"Execute"**

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "nik": "3201010101010001",
      "namaLengkap": "Ahmad Yani",
      "noKk": "3201010101010001",
      // ... more data
    }
  ],
  "message": "Success"
}
```

### **6.3 Get My Login Activities**

1. Scroll ke section: **LoginActivity**
2. Expand: `GET /api/LoginActivity/my-activities`
3. Click: **"Try it out"**
4. Optional: Change `limit` parameter (default: 10)
5. Click: **"Execute"**

**Response:**
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
  "message": "Login activities retrieved successfully",
  "total": 1
}
```

✅ **You can see your own login history!**

---

## 🔄 **STEP 7: Refresh Token (When Expired)**

**When:** After 60 minutes, `accessToken` will expire.

### **7.1 Use Refresh Token Endpoint**

1. Expand: `POST /api/Auth/refresh`
2. Click: **"Try it out"**
3. Input your saved `refreshToken`:

```json
{
  "refreshToken": "K9mP2qR5vB8nC1wD4eF7gH0jL3oM6pN9rS2tU5xY8zA..."
}
```

4. Click: **"Execute"**

### **7.2 Get New Access Token**

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.NEW_TOKEN_HERE...",
  "expiresIn": 3600
}
```

### **7.3 Update Authorization**

1. Copy new `accessToken`
2. Click: 🔒 **"Authorize"** (kanan atas)
3. Click: **"Logout"** (hapus token lama)
4. Paste new token with `Bearer ` prefix
5. Click: **"Authorize"**

✅ **You can continue testing with new token!**

---

## 🚪 **STEP 8: Logout (Optional)**

**When:** You want to end session and revoke refresh token.

1. Expand: `POST /api/Auth/logout`
2. Click: **"Try it out"**
3. Input:

```json
{
  "refreshToken": "K9mP2qR5vB8nC1wD4eF7gH0jL3oM6pN9rS2tU5xY8zA..."
}
```

4. Click: **"Execute"**

**Response:**
```json
{
  "message": "Logged out successfully"
}
```

✅ **Refresh token revoked! Session ended.**

---

## ❌ **BONUS: Test Error Scenarios**

### **Test 1: Weak Password**

**Endpoint:** `POST /api/Auth/register`

**Input:**
```json
{
  "username": "weak_test",
  "password": "weak",
  "email": "weak@test.com",
  "phoneNumber": "081999999991"
}
```

**Expected Error (400):**
```json
{
  "message": "Password minimal 8 karakter"
}
```

### **Test 2: Wrong Password (Trigger Lockout)**

**Endpoint:** `POST /api/Auth/login`

**Input (wrong password):**
```json
{
  "emailOrPhone": "john.doe@example.com",
  "password": "WrongPassword123!"
}
```

**Execute 5 times**

**Expected Error on 5th attempt (423):**
```json
{
  "message": "Akun dikunci selama 15 menit karena terlalu banyak percobaan login gagal.",
  "lockoutEnd": "2024-10-26T10:45:00Z",
  "remainingMinutes": 15
}
```

### **Test 3: Admin-Only Endpoint**

**Endpoint:** `GET /api/LoginActivity` (without `/my-activities`)

**Expected Error (403):**
```json
{
  "message": "Forbidden"
}
```

✅ **Security features working correctly!**

---

## 📊 **SWAGGER UI NAVIGATION TIPS**

### **Sections:**

- **🟢 Auth** - Authentication endpoints (Register, Login, Refresh, Logout)
- **🟢 Bpjs** - BPJS management (Get, Create, Update, Approve)
- **🟢 KtpKk** - KTP/KK management
- **🟢 LoginActivity** - Login history and audit logs

### **HTTP Methods Color Code:**

- **🟢 GET** - Read data (no changes)
- **🟡 POST** - Create new resource
- **🟠 PUT** - Update entire resource
- **🔵 PATCH** - Update partial resource
- **🔴 DELETE** - Remove resource

### **Authorization Icons:**

- **🔓 Open Padlock** - Not authorized yet
- **🔒 Locked Padlock** - Authorized with token
- **🔒 (gray)** - Endpoint doesn't need auth

### **Response Codes:**

- **2xx (Green)** - Success
  - 200 OK
  - 201 Created
- **4xx (Orange)** - Client error
  - 400 Bad Request
  - 401 Unauthorized
  - 403 Forbidden
  - 423 Locked
- **5xx (Red)** - Server error
  - 500 Internal Server Error

---

## 🎯 **COMPLETE TESTING CHECKLIST**

**Basic Flow:**
- ✅ Register User
- ✅ Login
- ✅ Authorize Swagger
- ✅ Get All BPJS
- ✅ Get All KTP/KK
- ✅ Get My Login Activities

**Advanced Flow:**
- ✅ Refresh Token (after 60 min)
- ✅ Logout

**Error Testing:**
- ✅ Test Weak Password
- ✅ Test Wrong Password (5x for lockout)
- ✅ Test Admin-Only Endpoint

---

## 💡 **PRO TIPS**

### **1. Keep Developer Console Open**
- Press **F12** (browser dev tools)
- Tab: **Console**
- Lihat network requests & errors

### **2. Use Browser's Find Feature**
- **Ctrl+F** untuk search endpoint
- Cepat navigate ke endpoint yang dicari

### **3. Save Token to Clipboard Manager**
- Use clipboard history (Win+V on Windows 10/11)
- Easy to paste token multiple times

### **4. Test with Different Data**
- Click **"Try it out"** lagi
- Edit JSON values
- Test different scenarios

### **5. Check Response Schema**
- Expand **"Schemas"** (bawah page)
- Lihat structure semua models
- Good for understanding data format

---

## 🚨 **COMMON ISSUES**

### **❌ "Failed to fetch"**

**Causes:**
- API not running
- Wrong port number
- CORS issue

**Solutions:**
1. Check `dotnet run` is running
2. Check URL matches console output port
3. Try HTTP instead of HTTPS

### **❌ 401 Unauthorized**

**Causes:**
- Token not set
- Token expired (after 60 min)
- Wrong token format

**Solutions:**
1. Re-login to get fresh token
2. Check `Bearer ` prefix (with space!)
3. Use refresh token endpoint

### **❌ Token too long, copy error**

**Solution:**
- Triple-click token text to select all
- Or use **"Copy"** button if available
- Don't select partial token

### **❌ "Authorization: undefined"**

**Solution:**
- Click 🔒 Authorize button again
- Re-paste token with `Bearer ` prefix
- Make sure no extra spaces/newlines

---

## 📂 **RECOMMENDED TESTING ORDER**

### **Day 1: Basic Setup**
1. Register user
2. Login
3. Get BPJS data
4. Get KTP/KK data
5. View login history

### **Day 2: Token Management**
1. Login
2. Use API for 1 hour (access different endpoints)
3. After 60 min, token expires
4. Use refresh token
5. Continue testing with new token
6. Logout when done

### **Day 3: Error Scenarios**
1. Test weak password validation
2. Test wrong password (trigger lockout)
3. Wait 15 minutes (or check database to reset)
4. Test admin-only endpoints (should fail)
5. Test invalid token

---

## 🌟 **ADVANTAGES OF SWAGGER**

### **✅ Pros:**
- Zero setup (built-in API documentation)
- Always up-to-date (auto-generated from code)
- Interactive testing (try immediately)
- Beautiful UI (easy to navigate)
- Schema documentation (see all models)
- No additional software needed

### **⚠️ Limitations:**
- No token persistence (need to re-authorize each session)
- No request collection/saving
- No automation/scripting
- No environment variables
- No team collaboration features

**For these needs, use Postman!**

See: `POSTMAN_QUICK_IMPORT.md`

---

## 🎓 **FOR ENTERPRISE APPLICATION SECURITY COURSE**

### **Security Features to Demonstrate:**

1. **Authentication & Authorization** ✅
   - JWT tokens
   - Role-based access (USER vs ADMIN)
   
2. **Password Security** ✅
   - Strong password validation
   - PBKDF2 hashing
   
3. **Account Protection** ✅
   - Account lockout after failed attempts
   - Rate limiting
   
4. **Session Management** ✅
   - Access token (60 min)
   - Refresh token (7 days)
   - Token revocation (logout)
   
5. **Audit Logging** ✅
   - Login activity tracking
   - IP address & device logging
   
6. **API Security** ✅
   - Bearer token authentication
   - Input validation
   - Generic error messages

**Test all these in Swagger UI!** 🎯

---

## 📞 **NEED MORE HELP?**

**Documentation:**
- `SWAGGER_POSTMAN_GUIDE.md` - Full Swagger & Postman guide
- `POSTMAN_QUICK_IMPORT.md` - Postman collection import
- `USER_TEST_CREDENTIALS.md` - All test data
- `USER_QUICK_REFERENCE.md` - Quick reference

**Logs:**
- API Console: Terminal window
- API Logs: `logs/sima-bpjs-*.log`
- Browser Console: F12 → Console

---

## ✨ **YOU'RE READY TO TEST!**

**Quick Start:**
1. `dotnet run` ← Start API
2. Open browser → `http://localhost:7189/swagger`
3. Register → Login → Copy Token → Authorize
4. Test all endpoints! ✅

**Total Time: ~5 minutes** ⏱️

---

_Happy Testing with Swagger UI! 🎨_

**Zero Setup → Instant Testing → Full Documentation!** 🚀

