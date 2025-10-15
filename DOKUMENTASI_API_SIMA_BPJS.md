# Dokumentasi API SIMA BPJS

## Deskripsi
API SIMA BPJS adalah sistem manajemen data BPJS yang menyediakan endpoint untuk:
- **Authentication** - Registrasi dan login pengguna
- **KTP/KK** - Manajemen data kependudukan
- **BPJS** - Manajemen data peserta BPJS
- **Pembayaran** - Manajemen pembayaran iuran BPJS

## Base URL
```
http://localhost:5189/api
```

## Authentication
API menggunakan JWT Bearer Token untuk autentikasi. Setelah login, gunakan token yang diterima di header Authorization.

---

## 1. AUTHENTICATION ENDPOINTS

### 1.1 Test API (Tidak Perlu Auth)
**GET** `/api/Auth/test`

**Response:**
```json
{
  "message": "API is working",
  "timestamp": "2025-01-27T10:30:00.000Z"
}
```

### 1.2 Register User
**POST** `/api/Auth/register`

**Request Body:**
```json
{
  "username": "john_doe",
  "password": "password123",
  "nik": "3201012345670001",
  "role": "USER"
}
```

**Contoh Data untuk Testing:**
```json
{
  "username": "admin_bpjs",
  "password": "admin123",
  "nik": "3201012345670001",
  "role": "ADMIN"
}
```

```json
{
  "username": "user_biasa",
  "password": "user123",
  "nik": "3201012345670002",
  "role": "USER"
}
```

**Response Success (201):**
```json
{
  "id": 1,
  "username": "john_doe",
  "role": "USER",
  "nik": "1234567890123456"
}
```

### 1.3 Login User
**POST** `/api/Auth/login`

**Request Body:**
```json
{
  "username": "john_doe",
  "password": "password123"
}
```

**Contoh Data untuk Testing:**
```json
{
  "username": "admin_bpjs",
  "password": "admin123"
}
```

**Response Success (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe",
  "role": "USER"
}
```

---

## 2. KTP/KK ENDPOINTS (Memerlukan Auth)

### 2.1 Get All KTP/KK
**GET** `/api/KtpKk`
**Headers:** `Authorization: Bearer {token}`

**Response Success (200):**
```json
[
  {
    "nik": "3201012345670001",
    "noKk": "3201012345670001",
    "namaLengkap": "Ahmad Wijaya",
    "tempatLahir": "Jakarta",
    "tanggalLahir": "1990-01-15T00:00:00",
    "jenisKelamin": "L",
    "alamat": "Jl. Merdeka No. 123, RT 001, RW 005, Jakarta Pusat",
    "agama": "Islam",
    "statusPerkawinan": "Kawin",
    "pekerjaan": "Pegawai Swasta",
    "kewarganegaraan": "WNI",
    "createdAt": "2025-01-27T10:30:00.000Z",
    "bpjs": []
  }
]
```

### 2.2 Get KTP/KK by NIK
**GET** `/api/KtpKk/{nik}`
**Headers:** `Authorization: Bearer {token}`

**Contoh URL:** `/api/KtpKk/3201012345670001`

### 2.3 Create KTP/KK
**POST** `/api/KtpKk`
**Headers:** `Authorization: Bearer {token}`

**Request Body:**
```json
{
  "nik": "3201012345670001",
  "noKk": "3201012345670001",
  "namaLengkap": "Ahmad Wijaya",
  "tempatLahir": "Jakarta",
  "tanggalLahir": "1990-01-15T00:00:00",
  "jenisKelamin": "L",
  "alamat": "Jl. Merdeka No. 123, RT 001, RW 005, Jakarta Pusat",
  "agama": "Islam",
  "statusPerkawinan": "Kawin",
  "pekerjaan": "Pegawai Swasta",
  "kewarganegaraan": "WNI"
}
```

**Contoh Data untuk Testing:**
```json
{
  "nik": "3201012345670002",
  "noKk": "3201012345670002",
  "namaLengkap": "Siti Nurhaliza",
  "tempatLahir": "Bandung",
  "tanggalLahir": "1992-03-20T00:00:00",
  "jenisKelamin": "P",
  "alamat": "Jl. Sudirman No. 456, RT 002, RW 003, Bandung",
  "agama": "Islam",
  "statusPerkawinan": "Belum Kawin",
  "pekerjaan": "Guru",
  "kewarganegaraan": "WNI"
}
```

### 2.4 Update KTP/KK
**PUT** `/api/KtpKk/{nik}`
**Headers:** `Authorization: Bearer {token}`

### 2.5 Delete KTP/KK
**DELETE** `/api/KtpKk/{nik}`
**Headers:** `Authorization: Bearer {token}`

---

## 3. BPJS ENDPOINTS (Memerlukan Auth)

### 3.1 Get All BPJS
**GET** `/api/Bpjs`
**Headers:** `Authorization: Bearer {token}`

**Response Success (200):**
```json
[
  {
    "idBpjs": 1,
    "nik": "3201012345670001",
    "noBpjs": "1234567890123",
    "faskesUtama": "RSUD Jakarta",
    "kelasPerawatan": "KELAS I",
    "statusPeserta": "AKTIF",
    "tanggalDaftar": "2025-01-27T00:00:00",
    "ktpKk": {
      "nik": "3201012345670001",
      "namaLengkap": "Ahmad Wijaya",
      "alamat": "Jl. Merdeka No. 123, RT 001, RW 005, Jakarta Pusat"
    },
    "pembayaran": []
  }
]
```

### 3.2 Get BPJS by ID
**GET** `/api/Bpjs/{id}`
**Headers:** `Authorization: Bearer {token}`

### 3.3 Create BPJS
**POST** `/api/Bpjs`
**Headers:** `Authorization: Bearer {token}`

**Request Body:**
```json
{
  "nik": "3201012345670001",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

**Contoh Data untuk Testing:**
```json
{
  "nik": "3201012345670002",
  "faskesUtama": "RS Siloam",
  "kelasPerawatan": "KELAS II",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

**Validasi Kelas Perawatan:**
- `KELAS I`
- `KELAS II` 
- `KELAS III`

**Response Success (201):**
```json
{
  "idBpjs": 1,
  "nik": "3201012345670001",
  "noBpjs": "1234567890123",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "statusPeserta": "PENDING",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### 3.4 Update BPJS
**PUT** `/api/Bpjs/{id}`
**Headers:** `Authorization: Bearer {token}`

### 3.5 Delete BPJS
**DELETE** `/api/Bpjs/{id}`
**Headers:** `Authorization: Bearer {token}`

### 3.6 Approve BPJS (Admin Only)
**POST** `/api/Bpjs/{id}/approve`
**Headers:** `Authorization: Bearer {token}`
**Role Required:** ADMIN

**Response Success (200):**
```json
{
  "message": "Peserta disetujui",
  "id": 1,
  "status": "AKTIF"
}
```

### 3.7 Deactivate BPJS (Admin Only)
**POST** `/api/Bpjs/{id}/deactivate`
**Headers:** `Authorization: Bearer {token}`
**Role Required:** ADMIN

**Response Success (200):**
```json
{
  "message": "Peserta dinonaktifkan",
  "id": 1,
  "status": "NONAKTIF"
}
```

---

## 4. PEMBAYARAN ENDPOINTS (Memerlukan Auth)

### 4.1 Get All Pembayaran
**GET** `/api/Pembayaran`
**Headers:** `Authorization: Bearer {token}`

**Response Success (200):**
```json
[
  {
    "idPembayaran": 1,
    "idBpjs": 1,
    "bulan": "Januari 2025",
    "tahun": 2025,
    "jumlah": 50000.00,
    "statusPembayaran": "LUNAS",
    "tanggalBayar": "2025-01-15T10:30:00.000Z",
    "bpjs": {
      "idBpjs": 1,
      "noBpjs": "1234567890123",
      "faskesUtama": "RSUD Jakarta"
    }
  }
]
```

### 4.2 Get Pembayaran by ID
**GET** `/api/Pembayaran/{id}`
**Headers:** `Authorization: Bearer {token}`

### 4.3 Create Pembayaran
**POST** `/api/Pembayaran`
**Headers:** `Authorization: Bearer {token}`

**Request Body:**
```json
{
  "idBpjs": 1,
  "bulan": "Januari 2025",
  "tahun": 2025,
  "jumlah": 50000.00,
  "statusPembayaran": "LUNAS",
  "tanggalBayar": "2025-01-15T10:30:00.000Z"
}
```

**Contoh Data untuk Testing:**
```json
{
  "idBpjs": 1,
  "bulan": "Februari 2025",
  "tahun": 2025,
  "jumlah": 50000.00,
  "statusPembayaran": "BELUM LUNAS",
  "tanggalBayar": null
}
```

**Status Pembayaran:**
- `LUNAS`
- `BELUM LUNAS`

### 4.4 Update Pembayaran
**PUT** `/api/Pembayaran/{id}`
**Headers:** `Authorization: Bearer {token}`

### 4.5 Delete Pembayaran
**DELETE** `/api/Pembayaran/{id}`
**Headers:** `Authorization: Bearer {token}`

---

## 5. CONTOH PENGGUNAAN DI POSTMAN

### 5.1 Setup Environment Variables
Buat environment di Postman dengan variabel:
- `base_url`: `https://localhost:7000/api`
- `token`: (akan diisi setelah login)

### 5.2 Collection untuk Testing

#### Step 1: Test API
```
GET {{base_url}}/Auth/test
```

#### Step 2: Register User
```
POST {{base_url}}/Auth/register
Content-Type: application/json

{
  "username": "test_user",
  "password": "test123",
  "nik": "1234567890123456",
  "role": "USER"
}
```

#### Step 3: Login
```
POST {{base_url}}/Auth/login
Content-Type: application/json

{
  "username": "test_user",
  "password": "test123"
}
```

**Setelah login, copy token dan set di environment variable `token`**

#### Step 4: Create KTP/KK
```
POST {{base_url}}/KtpKk
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "nik": "1234567890123456",
  "noKk": "1234567890123456",
  "namaLengkap": "John Doe",
  "tempatLahir": "Jakarta",
  "tanggalLahir": "1990-01-15T00:00:00",
  "jenisKelamin": "L",
  "alamat": "Jl. Merdeka No. 123, Jakarta Pusat",
  "agama": "Islam",
  "statusPerkawinan": "Belum Kawin",
  "pekerjaan": "Karyawan Swasta",
  "kewarganegaraan": "WNI"
}
```

#### Step 5: Create BPJS
```
POST {{base_url}}/Bpjs
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "nik": "1234567890123456",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

#### Step 6: Create Pembayaran
```
POST {{base_url}}/Pembayaran
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "idBpjs": 1,
  "bulan": "Januari 2025",
  "tahun": 2025,
  "jumlah": 50000.00,
  "statusPembayaran": "LUNAS",
  "tanggalBayar": "2025-01-15T10:30:00.000Z"
}
```

---

## 6. CONTOH PENGGUNAAN DI SWAGGER

1. Buka browser dan akses: `http://localhost:5189/swagger`
2. Klik "Authorize" di bagian atas
3. Masukkan token JWT dengan format: `Bearer {token}`
4. Klik "Authorize"
5. Sekarang Anda bisa menguji semua endpoint yang memerlukan autentikasi

---

## 7. ERROR RESPONSES

### 400 Bad Request
```json
{
  "message": "Username dan password wajib diisi"
}
```

### 401 Unauthorized
```json
{
  "message": "Kredensial tidak valid"
}
```

### 403 Forbidden
```json
{
  "message": "Access denied. Admin role required."
}
```

### 404 Not Found
```json
{
  "message": "Data tidak ditemukan"
}
```

### 409 Conflict
```json
{
  "message": "Username sudah digunakan"
}
```

### 500 Internal Server Error
```json
{
  "message": "Internal server error",
  "error": "Error details"
}
```

---

## 8. CATATAN PENTING

1. **JWT Token**: Token berlaku selama 60 menit (dapat dikonfigurasi di appsettings.json)
2. **Database**: Menggunakan MySQL dengan connection string di appsettings.json
3. **External API**: BPJS controller terintegrasi dengan API KTP eksternal di `https://ktp.chasouluix.biz.id/`
4. **Role-based Access**: Endpoint approve/deactivate BPJS hanya bisa diakses oleh user dengan role ADMIN
5. **Auto-generated**: Nomor BPJS di-generate otomatis berdasarkan NIK dan timestamp
6. **Status Flow**: BPJS status dimulai dari PENDING → AKTIF (setelah approve) → NONAKTIF (jika deactivate)

---

## 9. TESTING SCENARIOS

### Scenario 1: Registrasi dan Login
1. Test endpoint `/Auth/test` (tanpa auth)
2. Register user baru
3. Login dengan kredensial yang benar
4. Login dengan kredensial yang salah

### Scenario 2: CRUD KTP/KK
1. Create KTP/KK data
2. Get all KTP/KK
3. Get KTP/KK by NIK
4. Update KTP/KK data
5. Delete KTP/KK data

### Scenario 3: CRUD BPJS
1. Create BPJS (akan auto-create KTP jika belum ada)
2. Get all BPJS
3. Get BPJS by ID
4. Update BPJS
5. Approve BPJS (sebagai admin)
6. Deactivate BPJS (sebagai admin)

### Scenario 4: CRUD Pembayaran
1. Create pembayaran
2. Get all pembayaran
3. Get pembayaran by ID
4. Update pembayaran
5. Delete pembayaran

### Scenario 5: Error Handling
1. Test dengan token yang tidak valid
2. Test dengan role yang tidak sesuai
3. Test dengan data yang tidak valid
4. Test endpoint yang tidak ada
