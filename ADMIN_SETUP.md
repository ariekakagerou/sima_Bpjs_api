# 🔐 Admin Setup - SIMA BPJS API

## 🎯 Tujuan
Membuat admin yang hanya bisa login dan tidak bisa registrasi melalui endpoint biasa.

## ✅ Yang Sudah Diimplementasikan

### 1. **Auto Seed Admin** (Program.cs)
- Admin otomatis dibuat saat aplikasi start
- Username: `admin_bpjs`
- Password: `admin123`
- NIK: `3201012345670001`
- Role: `ADMIN`

### 2. **Restrict Register Admin** (AuthController.cs)
- User tidak bisa registrasi sebagai admin
- Error: "Tidak bisa mendaftar sebagai admin. Hubungi administrator untuk akses admin."

### 3. **Create Admin Endpoint** (Development Only)
- Endpoint: `POST /api/Auth/create-admin`
- Hanya tersedia saat development
- Memerlukan secret key: `ADMIN_SECRET_KEY_2025`

---

## 🚀 Cara Menggunakan

### 1. **Jalankan Aplikasi**
```bash
cd sima_Bpjs_api
dotnet run
```

### 2. **Admin Otomatis Dibuat**
Saat aplikasi start, admin akan otomatis dibuat:
```
✅ Admin berhasil dibuat: admin_bpjs / admin123
```

### 3. **Login sebagai Admin**
```json
POST http://localhost:5189/api/Auth/login
Content-Type: application/json

{
  "username": "admin_bpjs",
  "password": "admin123"
}
```

### 4. **Response Login**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin_bpjs",
  "role": "ADMIN"
}
```

---

## 🧪 Testing Admin Functions

### 1. **Approve BPJS**
```http
POST http://localhost:5189/api/Bpjs/4/approve
Authorization: Bearer {admin_token}
```

### 2. **Deactivate BPJS**
```http
POST http://localhost:5189/api/Bpjs/4/deactivate
Authorization: Bearer {admin_token}
```

### 3. **Get All BPJS**
```http
GET http://localhost:5189/api/Bpjs
Authorization: Bearer {admin_token}
```

---

## 🔧 Create Admin Tambahan (Development Only)

### 1. **Request Body**
```json
{
  "username": "admin2",
  "password": "admin123",
  "nik": "3201012345670002",
  "secretKey": "ADMIN_SECRET_KEY_2025"
}
```

### 2. **Response Success**
```json
{
  "message": "Admin berhasil dibuat",
  "username": "admin2",
  "role": "ADMIN",
  "nik": "3201012345670002"
}
```

---

## ⚠️ Keamanan

### 1. **Production Setup**
- Set `"IsDevelopment": false` di appsettings.json
- Endpoint create-admin akan disabled
- Hanya admin yang sudah ada yang bisa login

### 2. **Secret Key**
- Ganti secret key di production
- Simpan di environment variables
- Jangan commit ke repository

### 3. **Admin Password**
- Ganti password default di production
- Gunakan password yang kuat
- Update di Program.cs

---

## 🔄 Testing Flow

### 1. **Test Register sebagai Admin (Harus Error)**
```json
{
  "username": "test_admin",
  "password": "test123",
  "nik": "3201012345670001",
  "role": "ADMIN"
}
```

**Response Error:**
```json
{
  "message": "Tidak bisa mendaftar sebagai admin. Hubungi administrator untuk akses admin."
}
```

### 2. **Test Register sebagai User (Harus Success)**
```json
{
  "username": "test_user",
  "password": "test123",
  "nik": "3201012345670002",
  "role": "USER"
}
```

**Response Success:**
```json
{
  "id": 2,
  "username": "test_user",
  "role": "USER",
  "nik": "3201012345670002"
}
```

### 3. **Test Login Admin (Harus Success)**
```json
{
  "username": "admin_bpjs",
  "password": "admin123"
}
```

**Response Success:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin_bpjs",
  "role": "ADMIN"
}
```

---

## 📋 Daftar Admin

| Username | Password | NIK | Role | Status |
|----------|----------|-----|------|--------|
| admin_bpjs | admin123 | 3201012345670001 | ADMIN | ✅ Auto Created |

---

## 🎯 Kesimpulan

✅ **Admin hanya bisa login, tidak bisa registrasi**
✅ **User biasa tidak bisa registrasi sebagai admin**
✅ **Admin otomatis dibuat saat aplikasi start**
✅ **Admin bisa approve/deactivate BPJS**
✅ **Keamanan terjaga dengan secret key**

**Admin siap digunakan untuk mengelola peserta BPJS! 🎉**
