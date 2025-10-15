# SIMA BPJS API - CRUD Operations Documentation

## Overview
API ini menyediakan operasi CRUD (Create, Read, Update, Delete) lengkap untuk semua entitas dalam sistem SIMA BPJS.

## Base URL
```
https://localhost:7000/api
```

## Authentication
Semua endpoint memerlukan JWT token (kecuali login). Tambahkan header:
```
Authorization: Bearer <your-jwt-token>
```

## Endpoints

### 1. Admin Management

#### GET /api/Admin
Mengambil semua data admin (Admin only)
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "username": "admin_bpjs"
    }
  ],
  "message": "Admins retrieved successfully"
}
```

#### GET /api/Admin/{id}
Mengambil data admin berdasarkan ID (Admin only)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "username": "admin_bpjs"
  },
  "message": "Admin retrieved successfully"
}
```

#### POST /api/Admin
Membuat admin baru (Admin only)
```json
{
  "username": "new_admin",
  "password": 123456
}
```

#### PUT /api/Admin/{id}
Update data admin (Admin only)
```json
{
  "username": "updated_admin",
  "password": 654321
}
```

#### DELETE /api/Admin/{id}
Hapus data admin (Admin only)

### 2. KTP/KK Management

#### GET /api/KtpKk
Mengambil semua data KTP/KK
```json
{
  "success": true,
  "data": [
    {
      "nik": "3201012345670001",
      "noKk": "0000000000000000",
      "namaLengkap": "Ahmad Wijaya",
      "tempatLahir": "Jakarta",
      "tanggalLahir": "1990-01-15",
      "jenisKelamin": "L",
      "alamat": "Jl. Merdeka No. 123",
      "agama": "Islam",
      "statusPerkawinan": "Kawin",
      "pekerjaan": "Pegawai Swasta",
      "kewarganegaraan": "WNI",
      "createdAt": "2025-09-23T11:49:20Z"
    }
  ],
  "message": "KTP/KK data retrieved successfully"
}
```

#### GET /api/KtpKk/{nik}
Mengambil data KTP/KK berdasarkan NIK

#### POST /api/KtpKk
Membuat data KTP/KK baru
```json
{
  "nik": "3201012345670003",
  "noKk": "0000000000000001",
  "namaLengkap": "Siti Nurhaliza",
  "tempatLahir": "Bandung",
  "tanggalLahir": "1992-03-20",
  "jenisKelamin": "P",
  "alamat": "Jl. Sudirman No. 456",
  "agama": "Islam",
  "statusPerkawinan": "Belum Kawin",
  "pekerjaan": "Guru",
  "kewarganegaraan": "WNI"
}
```

#### PUT /api/KtpKk/{nik}
Update data KTP/KK
```json
{
  "noKk": "0000000000000001",
  "namaLengkap": "Siti Nurhaliza Updated",
  "tempatLahir": "Bandung",
  "tanggalLahir": "1992-03-20",
  "jenisKelamin": "P",
  "alamat": "Jl. Sudirman No. 456 Updated",
  "agama": "Islam",
  "statusPerkawinan": "Kawin",
  "pekerjaan": "Guru",
  "kewarganegaraan": "WNI"
}
```

#### DELETE /api/KtpKk/{nik}
Hapus data KTP/KK (akan ditolak jika ada data BPJS terkait)

### 3. BPJS Management

#### GET /api/Bpjs
Mengambil semua data BPJS
```json
{
  "success": true,
  "data": [
    {
      "idBpjs": 3,
      "nik": "3201012345670001",
      "noBpjs": "0001234567890",
      "faskesUtama": "RS Umum Daerah",
      "kelasPerawatan": "KELAS I",
      "statusPeserta": "AKTIF",
      "tanggalDaftar": "2024-01-15",
      "ktpKk": {
        "nik": "3201012345670001",
        "namaLengkap": "Ahmad Wijaya",
        // ... data KTP lengkap
      }
    }
  ],
  "message": "BPJS data retrieved successfully"
}
```

#### GET /api/Bpjs/{id}
Mengambil data BPJS berdasarkan ID

#### POST /api/Bpjs
Membuat data BPJS baru
```json
{
  "nik": "3201012345670003",
  "noBpjs": "0001234567891",
  "faskesUtama": "RS Siloam Bandung",
  "kelasPerawatan": "KELAS II",
  "statusPeserta": "PENDING",
  "tanggalDaftar": "2025-01-27"
}
```

#### PUT /api/Bpjs/{id}
Update data BPJS
```json
{
  "noBpjs": "0001234567891",
  "faskesUtama": "RS Siloam Bandung Updated",
  "kelasPerawatan": "KELAS I",
  "statusPeserta": "AKTIF",
  "tanggalDaftar": "2025-01-27"
}
```

#### DELETE /api/Bpjs/{id}
Hapus data BPJS (akan ditolak jika ada data pembayaran terkait)

#### POST /api/Bpjs/{id}/approve
Approve peserta BPJS menjadi AKTIF (Admin only)

#### POST /api/Bpjs/{id}/deactivate
Deactivate peserta BPJS menjadi NONAKTIF (Admin only)

### 4. Payment Management

#### GET /api/Pembayaran
Mengambil semua data pembayaran
```json
{
  "success": true,
  "data": [
    {
      "idPembayaran": 1,
      "idBpjs": 3,
      "bulan": "Januari 2025",
      "tahun": 2025,
      "jumlah": 50000.00,
      "statusPembayaran": "LUNAS",
      "tanggalBayar": "2025-01-15",
      "bpjs": {
        "idBpjs": 3,
        "nik": "3201012345670001",
        "noBpjs": "0001234567890",
        // ... data BPJS lengkap
      }
    }
  ],
  "message": "Payment data retrieved successfully"
}
```

#### GET /api/Pembayaran/{id}
Mengambil data pembayaran berdasarkan ID

#### POST /api/Pembayaran
Membuat data pembayaran baru
```json
{
  "idBpjs": 3,
  "bulan": "Februari 2025",
  "tahun": 2025,
  "jumlah": 50000.00,
  "statusPembayaran": "BELUM LUNAS",
  "tanggalBayar": null
}
```

#### PUT /api/Pembayaran/{id}
Update data pembayaran
```json
{
  "bulan": "Februari 2025",
  "tahun": 2025,
  "jumlah": 50000.00,
  "statusPembayaran": "LUNAS",
  "tanggalBayar": "2025-02-15"
}
```

#### DELETE /api/Pembayaran/{id}
Hapus data pembayaran

### 5. User Management

#### GET /api/User
Mengambil semua data user (Admin only)
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "username": "admin_bpjs",
      "email": null,
      "phoneNumber": "08123456789",
      "role": "ADMIN",
      "nik": "3201012345670001",
      "dateOfBirth": "2015-09-02",
      "createdAt": "2025-09-23T19:04:08Z"
    }
  ],
  "message": "Users retrieved successfully"
}
```

#### GET /api/User/{id}
Mengambil data user berdasarkan ID

#### POST /api/User
Membuat user baru (Admin only)
```json
{
  "username": "new_user",
  "password": "password123",
  "email": "user@example.com",
  "phoneNumber": "081234567890",
  "role": "USER",
  "nik": "3201012345670006",
  "dateOfBirth": "1990-01-01"
}
```

#### PUT /api/User/{id}
Update data user
```json
{
  "username": "updated_user",
  "email": "updated@example.com",
  "phoneNumber": "081234567890",
  "role": "USER",
  "nik": "3201012345670006",
  "dateOfBirth": "1990-01-01"
}
```

#### PUT /api/User/{id}/password
Update password user
```json
{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword123"
}
```

#### DELETE /api/User/{id}
Hapus data user (Admin only, akan ditolak jika ada data BPJS terkait)

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Invalid data",
  "errors": {
    "fieldName": ["Error message"]
  }
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Unauthorized"
}
```

### 403 Forbidden
```json
{
  "success": false,
  "message": "Forbidden"
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Resource not found"
}
```

### 409 Conflict
```json
{
  "success": false,
  "message": "Resource already exists"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "Internal server error",
  "error": "Error details"
}
```

## Validation Rules

### KTP/KK
- NIK: Required, exactly 16 characters
- No KK: Required, exactly 16 characters
- Nama Lengkap: Required, max 100 characters
- Jenis Kelamin: L or P only

### BPJS
- NIK: Required, exactly 16 characters
- No BPJS: Required, max 20 characters, unique
- Kelas Perawatan: KELAS I, KELAS II, or KELAS III
- Status Peserta: PENDING, AKTIF, or NONAKTIF

### Pembayaran
- ID BPJS: Required, must exist
- Bulan: Required, max 20 characters
- Tahun: Required, between 2000-2100
- Jumlah: Required, greater than 0
- Status Pembayaran: LUNAS or BELUM LUNAS

### User
- Username: Required, max 100 characters, unique
- Password: Required, min 6 characters
- Email: Valid email format, unique (if provided)
- Phone Number: Valid phone format, unique (if provided)
- Role: USER or ADMIN
- NIK: Exactly 16 characters, unique (if provided)

## Business Rules

1. **Cascade Delete Protection**: 
   - KTP/KK cannot be deleted if it has related BPJS records
   - BPJS cannot be deleted if it has related payment records
   - User cannot be deleted if it has related BPJS records

2. **Unique Constraints**:
   - Username must be unique across all users
   - Email must be unique (if provided)
   - Phone number must be unique (if provided)
   - NIK must be unique (if provided)
   - No BPJS must be unique

3. **Data Integrity**:
   - BPJS records must reference existing KTP/KK records
   - Payment records must reference existing BPJS records
   - All foreign key relationships are enforced

4. **Authorization**:
   - Admin operations require ADMIN role
   - Users can only update their own data (except admins)
   - Password updates require current password verification
