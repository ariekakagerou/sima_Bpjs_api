# ⚡ POSTMAN QUICK IMPORT GUIDE ⚡

## 🚀 Get Started in 3 Minutes!

Import Postman collection dan environment **TANPA setup manual**!

---

## 📦 **STEP 1: Import Collection**

### **Method 1: Drag & Drop (Termudah!)**

1. Open **Postman**
2. Drag file ini ke window Postman:
   ```
   SIMA_BPJS_USER_TESTING.postman_collection.json
   ```
3. ✅ Collection imported!

### **Method 2: Manual Import**

1. Open Postman
2. Click: **"Import"** (kiri atas)
3. Click: **"Upload Files"**
4. Select: `SIMA_BPJS_USER_TESTING.postman_collection.json`
5. Click: **"Import"**
6. ✅ Done!

---

## 🌍 **STEP 2: Import Environment**

### **Drag & Drop:**

1. Drag file ini ke Postman:
   ```
   SIMA_BPJS_Local.postman_environment.json
   ```
2. ✅ Environment imported!

### **Manual Import:**

1. Click: **"Import"**
2. Select: `SIMA_BPJS_Local.postman_environment.json`
3. Click: **"Import"**
4. ✅ Done!

---

## ⚙️ **STEP 3: Activate Environment**

1. Look di **kanan atas** Postman
2. Click dropdown: **"No Environment"**
3. Select: **"SIMA BPJS Local"**
4. ✅ Environment active!

---

## 🎯 **STEP 4: Update Base URL (PENTING!)**

### **Check API Port:**

Jalankan API dulu untuk tahu port number:

```powershell
cd c:/Users/LENOVO/BPJS(sima)/sima_Bpjs_api
dotnet run
```

Lihat output:
```
Now listening on: http://localhost:7189
```

**Port bisa berbeda: 5189, 7189, 5000, dll!**

### **Update Environment:**

1. Click: ⚙️ **"Environments"** (sidebar kiri)
2. Click: **"SIMA BPJS Local"**
3. Update `base_url` value sesuai port yang muncul:
   - Jika port `5189`: `http://localhost:5189`
   - Jika port `7189`: `http://localhost:7189`
   - Jika port `5000`: `http://localhost:5000`
4. Click: **"Save"** (Ctrl+S)

---

## ✅ **STEP 5: Test Collection!**

### **5.1 Expand Collection**

1. Click: **"SIMA BPJS API - User Testing"** (sidebar kiri)
2. Expand: **"1. Authentication"**

### **5.2 Register User**

1. Click: **"Register User - John Doe"**
2. Click: **"Send"** (biru besar)
3. Lihat response: **Status 201 Created**
4. ✅ User created!

### **5.3 Login (Auto-Save Token!)**

1. Click: **"Login with Email"**
2. Click: **"Send"**
3. Lihat response: **Status 200 OK**
4. **MAGIC:** Token otomatis tersimpan! 🎉

**Check Token:**
- Click: 👁️ **"Environment quick look"** (kanan atas)
- Lihat `access_token` dan `refresh_token` sudah terisi!

### **5.4 Test Authenticated Endpoint**

1. Expand: **"2. BPJS Management"**
2. Click: **"Get All BPJS"**
3. Click: **"Send"**
4. ✅ Lihat data BPJS! (Token otomatis di-attach!)

---

## 🎯 **COMPLETE TESTING FLOW**

Run requests in order:

### **Authentication Flow:**
```
1. Register User - John Doe        → Creates user
2. Login with Email                → Gets & saves tokens
3. Get My Login Activities         → View login history
4. Refresh Token                   → Update access token
5. Logout                          → Revoke refresh token
```

### **Data Access Flow:**
```
1. Get All BPJS                    → View all BPJS
2. Get BPJS by ID                  → View specific BPJS
3. Get All KTP/KK                  → View KTP/KK data
```

### **Error Testing Flow:**
```
1. Test Weak Password              → Should fail (400)
2. Test Wrong Password             → Should fail (401)
3. Test Admin-Only Endpoint        → Should fail (403)
```

---

## 🔄 **AUTO-TOKEN MANAGEMENT** (Built-in!)

Collection sudah dilengkapi **auto-save scripts**:

### **What's Auto-Saved:**
- ✅ `access_token` - Saved on login
- ✅ `refresh_token` - Saved on login
- ✅ `username` - Saved on login
- ✅ Auto-update `access_token` on refresh

### **How it Works:**

**Login Request** memiliki Tests script:
```javascript
if (pm.response.code === 200) {
    const response = pm.response.json();
    pm.environment.set("access_token", response.accessToken);
    pm.environment.set("refresh_token", response.refreshToken);
    pm.environment.set("username", response.username);
    console.log("✅ Tokens saved!");
}
```

**Refresh Request** auto-update access token:
```javascript
if (pm.response.code === 200) {
    const response = pm.response.json();
    pm.environment.set("access_token", response.accessToken);
    console.log("✅ Access token refreshed!");
}
```

---

## 📊 **COLLECTION STRUCTURE**

```
SIMA BPJS API - User Testing
│
├── 1. Authentication
│   ├── Register User - John Doe
│   ├── Login with Email           ← Auto-save tokens
│   ├── Login with Phone
│   ├── Refresh Token              ← Auto-update token
│   └── Logout
│
├── 2. BPJS Management
│   ├── Get All BPJS               ← Uses auto-saved token
│   └── Get BPJS by ID
│
├── 3. KTP/KK Management
│   └── Get All KTP/KK
│
├── 4. Login Activities
│   └── Get My Login Activities
│
└── 5. Error Testing
    ├── Test Weak Password         ← Should return 400
    ├── Test Wrong Password        ← Should return 401
    └── Test Admin-Only Endpoint   ← Should return 403
```

---

## 🌐 **ENVIRONMENT VARIABLES**

Collection menggunakan variables ini (auto-managed):

| Variable         | Purpose                          | Auto-Fill? |
|------------------|----------------------------------|------------|
| `base_url`       | API base URL                     | ❌ Manual  |
| `base_url_https` | API HTTPS URL                    | ❌ Manual  |
| `access_token`   | JWT access token (60 min)        | ✅ Yes     |
| `refresh_token`  | JWT refresh token (7 days)       | ✅ Yes     |
| `username`       | Current logged-in username       | ✅ Yes     |

**You only need to set:** `base_url` (based on your API port)

---

## 💡 **PRO TIPS**

### **1. Check Console for Debug Info**
- Click: **"Console"** (bottom left)
- Lihat logs dari auto-save scripts
- Lihat token yang di-save

### **2. Use Quick Look for Token Status**
- Click: 👁️ icon (kanan atas)
- Lihat current values of variables
- Check if tokens are saved

### **3. Run Multiple Requests**
- Select folder: **"1. Authentication"**
- Click: **"Run"** (kanan atas)
- Run all authentication flows at once

### **4. Duplicate for Different Users**
- Right-click: **"Register User - John Doe"**
- Click: **"Duplicate"**
- Edit body untuk user baru

### **5. Save Response Examples**
- Setelah send request dengan success
- Click: **"Save Response"** (di bawah response)
- Click: **"Save as Example"**
- Good for documentation!

---

## 🚨 **TROUBLESHOOTING**

### **❌ Problem: "Could not get any response"**

**Solution:**
1. Check API is running: `dotnet run`
2. Check port number matches `base_url`
3. Try HTTP instead of HTTPS

### **❌ Problem: Variables not working**

**Example:** Request shows `{{base_url}}` instead of URL

**Solution:**
1. Check environment is selected (dropdown kanan atas)
2. Make sure `base_url` has value in environment
3. Hover over `{{base_url}}` in request - should show resolved value

### **❌ Problem: 401 Unauthorized**

**Solution:**
1. Run "Login with Email" again to get fresh tokens
2. Check `access_token` in environment (should not be empty)
3. Check request has Authorization tab set to "Inherit from parent"

### **❌ Problem: Token not auto-saved**

**Solution:**
1. Open **Console** (bottom left)
2. Look for errors in test script
3. Make sure response is 200/201
4. Check Tests tab in request has script

### **❌ Problem: 403 Forbidden on BPJS/KTP endpoints**

**This is EXPECTED for USER role!**

USER role cannot access admin-only endpoints:
- `POST /api/bpjs/{id}/approve`
- `GET /api/loginactivity` (all users)
- etc.

---

## 📞 **NEED MORE HELP?**

**Check these files:**
- `SWAGGER_POSTMAN_GUIDE.md` - Complete Swagger & Postman guide
- `USER_TEST_CREDENTIALS.md` - All test data & credentials
- `USER_QUICK_REFERENCE.md` - Quick reference card

**Check Logs:**
- Postman Console: Bottom left
- API Logs: `logs/sima-bpjs-*.log`
- API Output: Terminal window

---

## 📝 **QUICK CHECKLIST**

Before testing, make sure:

- ✅ API is running (`dotnet run`)
- ✅ MySQL is running
- ✅ Collection imported
- ✅ Environment imported
- ✅ Environment selected (dropdown kanan atas)
- ✅ `base_url` matches API port
- ✅ Database migrated (`dotnet ef database update`)

---

## 🎉 **YOU'RE READY!**

Collection sudah **fully configured** dengan:
- ✅ Pre-configured requests
- ✅ Auto-token management
- ✅ Test scripts
- ✅ Validation tests
- ✅ Error scenarios
- ✅ Complete documentation

**Just import and start testing!** 🚀

---

## 📂 **FILES YOU NEED**

Import these 2 files to Postman:

1. **Collection:** `SIMA_BPJS_USER_TESTING.postman_collection.json`
2. **Environment:** `SIMA_BPJS_Local.postman_environment.json`

**That's it!** No manual setup needed! 🎯

---

_Happy Testing with Postman! 📬_

**Import → Run → Test → Done!** ✨

