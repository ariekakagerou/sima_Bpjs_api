# 🧪 Admin Testing Examples

## 🔐 Login Admin

### Request
```http
POST http://localhost:5189/api/Auth/login
Content-Type: application/json

{
  "username": "admin_bpjs",
  "password": "admin123"
}
```

### Response Success
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5fYnBqcyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFETUlOIiwibmJmIjoxNzM4MDA5NjAwLCJleHAiOjE3MzgwMTMyMDAsImlzcyI6InNpbWFfYnBqc19hcGkiLCJhdWQiOiJzaW1hX2JwanNfYXBpX3VzZXJzIn0.example",
  "username": "admin_bpjs",
  "role": "ADMIN"
}
```

---

## 🏥 Admin Functions

### 1. **Approve BPJS (PENDING → AKTIF)**

#### Request
```http
POST http://localhost:5189/api/Bpjs/4/approve
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Response Success
```json
{
  "message": "Peserta disetujui",
  "id": 4,
  "status": "AKTIF"
}
```

### 2. **Deactivate BPJS (AKTIF → NONAKTIF)**

#### Request
```http
POST http://localhost:5189/api/Bpjs/4/deactivate
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Response Success
```json
{
  "message": "Peserta dinonaktifkan",
  "id": 4,
  "status": "NONAKTIF"
}
```

### 3. **Get All BPJS**

#### Request
```http
GET http://localhost:5189/api/Bpjs
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### Response Success
```json
[
  {
    "idBpjs": 4,
    "nik": "3201012345670002",
    "noBpjs": "4787525670002",
    "faskesUtama": "RS Siloam Bandung",
    "kelasPerawatan": "KELAS II",
    "statusPeserta": "AKTIF",
    "tanggalDaftar": "2025-01-27T00:00:00",
    "ktpKk": {
      "nik": "3201012345670002",
      "namaLengkap": "Siti Nurhaliza",
      "alamat": "Jl. Sudirman No. 456, RT 002, RW 003, Bandung"
    },
    "pembayaran": []
  }
]
```

---

## 🚫 Error Testing

### 1. **Register sebagai Admin (Harus Error)**

#### Request
```http
POST http://localhost:5189/api/Auth/register
Content-Type: application/json

{
  "username": "test_admin",
  "password": "test123",
  "nik": "3201012345670001",
  "role": "ADMIN"
}
```

#### Response Error
```json
{
  "message": "Tidak bisa mendaftar sebagai admin. Hubungi administrator untuk akses admin."
}
```

### 2. **Access Admin Function tanpa Token (Harus Error)**

#### Request
```http
POST http://localhost:5189/api/Bpjs/4/approve
```

#### Response Error
```json
{
  "message": "Unauthorized"
}
```

### 3. **Access Admin Function dengan User Token (Harus Error)**

#### Request
```http
POST http://localhost:5189/api/Bpjs/4/approve
Authorization: Bearer {user_token}
```

#### Response Error
```json
{
  "message": "Access denied. Admin role required."
}
```

---

## 🔧 Create Admin Tambahan (Development Only)

### Request
```http
POST http://localhost:5189/api/Auth/create-admin
Content-Type: application/json

{
  "username": "admin2",
  "password": "admin123",
  "nik": "3201012345670002",
  "secretKey": "ADMIN_SECRET_KEY_2025"
}
```

### Response Success
```json
{
  "message": "Admin berhasil dibuat",
  "username": "admin2",
  "role": "ADMIN",
  "nik": "3201012345670002"
}
```

---

## 🧪 Testing Flow Lengkap

### Step 1: Login Admin
```bash
curl -X POST "http://localhost:5189/api/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin_bpjs",
    "password": "admin123"
  }'
```

### Step 2: Set Token
```bash
export ADMIN_TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Step 3: Get All BPJS
```bash
curl -X GET "http://localhost:5189/api/Bpjs" \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

### Step 4: Approve BPJS
```bash
curl -X POST "http://localhost:5189/api/Bpjs/4/approve" \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

### Step 5: Deactivate BPJS
```bash
curl -X POST "http://localhost:5189/api/Bpjs/4/deactivate" \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

---

## 📊 Status Flow Testing

### Test Case 1: PENDING → AKTIF
1. Create BPJS (status: PENDING)
2. Login sebagai admin
3. Approve BPJS
4. Verify status: AKTIF

### Test Case 2: AKTIF → NONAKTIF
1. BPJS dengan status: AKTIF
2. Login sebagai admin
3. Deactivate BPJS
4. Verify status: NONAKTIF

### Test Case 3: NONAKTIF → AKTIF
1. BPJS dengan status: NONAKTIF
2. Login sebagai admin
3. Approve BPJS
4. Verify status: AKTIF

---

## ⚠️ Catatan Penting

1. **Token Expired:** Token berlaku 60 menit
2. **Role Required:** Hanya admin yang bisa approve/deactivate
3. **Development Only:** Create admin hanya tersedia saat development
4. **Secret Key:** Ganti secret key di production
5. **Password:** Ganti password default di production

---

**Gunakan contoh ini untuk testing admin functions! 🎯**
