# 🚀 Admin Quick Start - SIMA BPJS API

## ✅ Yang Sudah Diimplementasikan

### 1. **Auto Seed Admin**
- Admin otomatis dibuat saat aplikasi start
- Username: `admin_bpjs`
- Password: `admin123`
- NIK: `3201012345670001`
- Role: `ADMIN`

### 2. **Restrict Register Admin**
- User tidak bisa registrasi sebagai admin
- Error: "Tidak bisa mendaftar sebagai admin. Hubungi administrator untuk akses admin."

### 3. **Admin Functions**
- Approve BPJS (PENDING → AKTIF)
- Deactivate BPJS (AKTIF → NONAKTIF)
- Get All BPJS
- Get All Pembayaran

---

## 🚀 Cara Cepat Mulai

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

### 5. **Set Authorization**
- **Swagger UI:** Klik "Authorize" → `Bearer {token}`
- **Postman:** Set di environment variable

---

## 🏥 Admin Functions

### 1. **Approve BPJS**
```http
POST http://localhost:5189/api/Bpjs/4/approve
Authorization: Bearer {admin_token}
```

**Response:**
```json
{
  "message": "Peserta disetujui",
  "id": 4,
  "status": "AKTIF"
}
```

### 2. **Deactivate BPJS**
```http
POST http://localhost:5189/api/Bpjs/4/deactivate
Authorization: Bearer {admin_token}
```

**Response:**
```json
{
  "message": "Peserta dinonaktifkan",
  "id": 4,
  "status": "NONAKTIF"
}
```

### 3. **Get All BPJS**
```http
GET http://localhost:5189/api/Bpjs
Authorization: Bearer {admin_token}
```

---

## 🧪 Testing di Swagger UI

### 1. **Buka Swagger**
```
http://localhost:5189/swagger
```

### 2. **Login Admin**
- Pilih `POST /api/Auth/login`
- Masukkan data admin
- Copy token

### 3. **Set Authorization**
- Klik "Authorize"
- Masukkan: `Bearer {token}`
- Klik "Authorize"

### 4. **Test Admin Functions**
- `POST /api/Bpjs/{id}/approve`
- `POST /api/Bpjs/{id}/deactivate`
- `GET /api/Bpjs`

---

## 🧪 Testing di Postman

### 1. **Import Collection**
```
SIMA_BPJS_API_POSTMAN_COLLECTION.http
```

### 2. **Setup Environment**
- `base_url`: `http://localhost:5189/api`
- `admin_token`: (kosong dulu)

### 3. **Login Admin**
- Jalankan `POST /api/Auth/login`
- Copy token ke environment

### 4. **Test Admin Functions**
- `POST /api/Bpjs/4/approve`
- `POST /api/Bpjs/4/deactivate`
- `GET /api/Bpjs`

---

## 🚫 Error Testing

### 1. **Register sebagai Admin (Harus Error)**
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

### 2. **Access Admin Function tanpa Token (Harus Error)**
```http
POST http://localhost:5189/api/Bpjs/4/approve
```

**Response Error:**
```json
{
  "message": "Unauthorized"
}
```

### 3. **Access Admin Function dengan User Token (Harus Error)**
```http
POST http://localhost:5189/api/Bpjs/4/approve
Authorization: Bearer {user_token}
```

**Response Error:**
```json
{
  "message": "Access denied. Admin role required."
}
```

---

## 🔧 Create Admin Tambahan (Development Only)

### Request
```json
{
  "username": "admin2",
  "password": "admin123",
  "nik": "3201012345670002",
  "secretKey": "ADMIN_SECRET_KEY_2025"
}
```

### Response
```json
{
  "message": "Admin berhasil dibuat",
  "username": "admin2",
  "role": "ADMIN",
  "nik": "3201012345670002"
}
```

---

## 📊 Status Flow

```
PENDING → [Admin Approve] → AKTIF
PENDING → [Admin Deactivate] → NONAKTIF
AKTIF → [Admin Deactivate] → NONAKTIF
NONAKTIF → [Admin Approve] → AKTIF
```

---

## ⚠️ Keamanan

### 1. **Production Setup**
- Set `"IsDevelopment": false` di appsettings.json
- Endpoint create-admin akan disabled
- Hanya admin yang sudah ada yang bisa login

### 2. **Admin Password**
- Ganti password default di production
- Update di Program.cs

### 3. **Secret Key**
- Ganti secret key di production
- Simpan di environment variables

---

## 🎯 Kesimpulan

✅ **Admin otomatis dibuat saat aplikasi start**
✅ **User tidak bisa registrasi sebagai admin**
✅ **Admin bisa approve/deactivate BPJS**
✅ **Keamanan terjaga dengan role-based access**
✅ **Testing lengkap tersedia di Swagger dan Postman**

**Admin siap digunakan untuk mengelola peserta BPJS! 🎉**

---

## 📁 File Referensi

- `ADMIN_SETUP.md` - Dokumentasi lengkap admin setup
- `ADMIN_TESTING_EXAMPLES.md` - Contoh testing admin
- `SIMA_BPJS_API_POSTMAN_COLLECTION.http` - Collection Postman
- `Program.cs` - Auto seed admin
- `AuthController.cs` - Restrict register admin
