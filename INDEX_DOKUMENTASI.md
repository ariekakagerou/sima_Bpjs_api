# 📚 INDEX DOKUMENTASI API SIMA BPJS

## 🎯 Tujuan
Dokumentasi lengkap untuk testing API SIMA BPJS menggunakan Swagger UI dan Postman.

---

## 📋 DAFTAR FILE DOKUMENTASI

### 🚀 **QUICK START**
| File | Deskripsi | Kapan Digunakan |
|------|-----------|-----------------|
| [QUICK_START.md](./QUICK_START.md) | Mulai testing dalam 5 menit | **PERTAMA KALI** |
| [SWAGGER_EXAMPLES.md](./SWAGGER_EXAMPLES.md) | Contoh data untuk Swagger UI | **TESTING SWAGGER** |

### 📖 **DOKUMENTASI LENGKAP**
| File | Deskripsi | Kapan Digunakan |
|------|-----------|-----------------|
| [DOKUMENTASI_API_SIMA_BPJS.md](./DOKUMENTASI_API_SIMA_BPJS.md) | Dokumentasi lengkap API | **REFERENSI LENGKAP** |
| [PANDUAN_TESTING.md](./PANDUAN_TESTING.md) | Panduan testing detail | **TESTING MENDALAM** |

### 🔧 **FILE TESTING**
| File | Deskripsi | Kapan Digunakan |
|------|-----------|-----------------|
| [SIMA_BPJS_API_POSTMAN_COLLECTION.http](./SIMA_BPJS_API_POSTMAN_COLLECTION.http) | Collection Postman | **TESTING POSTMAN** |
| [CONTOH_DATA_JSON.json](./CONTOH_DATA_JSON.json) | Data testing JSON | **REFERENSI DATA** |

### 📊 **NAVIGASI**
| File | Deskripsi | Kapan Digunakan |
|------|-----------|-----------------|
| [README_TESTING.md](./README_TESTING.md) | Index utama dokumentasi | **NAVIGASI** |
| [INDEX_DOKUMENTASI.md](./INDEX_DOKUMENTASI.md) | File ini | **OVERVIEW** |

---

## 🚀 PILIH CARA TESTING

### Option 1: Swagger UI (Termudah)
```
1. Buka QUICK_START.md
2. Ikuti langkah 1-7
3. Gunakan SWAGGER_EXAMPLES.md untuk data
```

### Option 2: Postman (Lebih Lengkap)
```
1. Buka PANDUAN_TESTING.md
2. Import SIMA_BPJS_API_POSTMAN_COLLECTION.http
3. Setup environment
4. Test endpoint
```

### Option 3: Referensi Lengkap
```
1. Buka DOKUMENTASI_API_SIMA_BPJS.md
2. Baca semua endpoint
3. Gunakan CONTOH_DATA_JSON.json
```

---

## 📊 STRUKTUR API

### 🔐 Authentication
- `GET /api/Auth/test` - Test API (no auth)
- `POST /api/Auth/register` - Register user
- `POST /api/Auth/login` - Login user

### 👤 KTP/KK Management
- `GET /api/KtpKk` - Get all KTP/KK
- `GET /api/KtpKk/{nik}` - Get by NIK
- `POST /api/KtpKk` - Create KTP/KK
- `PUT /api/KtpKk/{nik}` - Update KTP/KK
- `DELETE /api/KtpKk/{nik}` - Delete KTP/KK

### 🏥 BPJS Management
- `GET /api/Bpjs` - Get all BPJS
- `GET /api/Bpjs/{id}` - Get by ID
- `POST /api/Bpjs` - Create BPJS
- `PUT /api/Bpjs/{id}` - Update BPJS
- `DELETE /api/Bpjs/{id}` - Delete BPJS
- `POST /api/Bpjs/{id}/approve` - Approve BPJS (Admin)
- `POST /api/Bpjs/{id}/deactivate` - Deactivate BPJS (Admin)

### 💰 Payment Management
- `GET /api/Pembayaran` - Get all payments
- `GET /api/Pembayaran/{id}` - Get by ID
- `POST /api/Pembayaran` - Create payment
- `PUT /api/Pembayaran/{id}` - Update payment
- `DELETE /api/Pembayaran/{id}` - Delete payment

---

## 🔐 AUTHENTICATION

### JWT Bearer Token
1. Register/Login untuk mendapat token
2. Gunakan di header: `Authorization: Bearer {token}`
3. Token berlaku 60 menit

### Roles
- **USER**: Akses CRUD data biasa
- **ADMIN**: Akses semua endpoint + approve/deactivate BPJS

---

## 📝 CONTOH DATA CEPAT

### User Admin
```json
{
  "username": "admin_bpjs",
  "password": "admin123",
  "nik": "1234567890123456",
  "role": "ADMIN"
}
```

### User Biasa
```json
{
  "username": "user_biasa",
  "password": "user123",
  "nik": "9876543210987654",
  "role": "USER"
}
```

### BPJS Data
```json
{
  "nik": "1234567890123456",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

---

## 🧪 TESTING SCENARIOS

### 1. Happy Path
- ✅ Register → Login → Create Data → Get Data

### 2. Error Handling
- ✅ Invalid credentials
- ✅ Missing required fields
- ✅ Invalid data format
- ✅ Unauthorized access

### 3. Role-based Access
- ✅ Admin functions
- ✅ User restrictions
- ✅ Permission validation

### 4. Data Validation
- ✅ Required fields
- ✅ Data format
- ✅ Business rules

---

## 🚨 COMMON ISSUES

| Issue | Solution | File Referensi |
|-------|----------|----------------|
| Connection refused | Pastikan aplikasi running di port 7000 | QUICK_START.md |
| Unauthorized | Login dulu untuk mendapat token | SWAGGER_EXAMPLES.md |
| Forbidden | Gunakan role admin untuk endpoint admin | PANDUAN_TESTING.md |
| Not Found | Pastikan data sudah ada | DOKUMENTASI_API_SIMA_BPJS.md |
| Bad Request | Cek format JSON dan required fields | CONTOH_DATA_JSON.json |

---

## 📈 WORKFLOW TESTING

### Step 1: Persiapan
1. Jalankan aplikasi: `dotnet run`
2. Buka Swagger: `http://localhost:5189/swagger`
3. Atau import Postman collection

### Step 2: Authentication
1. Test API tanpa auth
2. Register user
3. Login user
4. Set authorization

### Step 3: CRUD Operations
1. Create KTP/KK
2. Create BPJS
3. Create Pembayaran
4. Test semua GET endpoints

### Step 4: Admin Functions
1. Login sebagai admin
2. Test approve/deactivate BPJS
3. Test role-based access

### Step 5: Error Testing
1. Test invalid data
2. Test unauthorized access
3. Test missing fields

---

## 📞 SUPPORT

### Jika Mengalami Masalah
1. **Cek log aplikasi** di console
2. **Baca troubleshooting** di PANDUAN_TESTING.md
3. **Pastikan database** MySQL running
4. **Cek koneksi internet** untuk external API

### File Bantuan
- `PANDUAN_TESTING.md` - Troubleshooting detail
- `DOKUMENTASI_API_SIMA_BPJS.md` - Referensi lengkap
- `QUICK_START.md` - Solusi cepat

---

## 🎯 REKOMENDASI PENGGUNAAN

### Untuk Pemula
1. Baca `QUICK_START.md`
2. Gunakan `SWAGGER_EXAMPLES.md`
3. Test dengan Swagger UI

### Untuk Developer
1. Baca `DOKUMENTASI_API_SIMA_BPJS.md`
2. Import `SIMA_BPJS_API_POSTMAN_COLLECTION.http`
3. Gunakan `CONTOH_DATA_JSON.json`

### Untuk Testing Mendalam
1. Baca `PANDUAN_TESTING.md`
2. Ikuti semua testing scenarios
3. Test error handling

---

## 📊 STATISTIK DOKUMENTASI

- **Total File**: 8 file
- **Total Endpoint**: 20+ endpoint
- **Contoh Data**: 50+ contoh
- **Testing Scenarios**: 5 skenario
- **Error Cases**: 10+ error cases

---

**Happy Testing! 🎉**

*Dokumentasi ini dibuat untuk memudahkan testing API SIMA BPJS. Pilih file yang sesuai dengan kebutuhan Anda dan mulai testing!*
