# 📚 Dokumentasi Testing API SIMA BPJS

## 🎯 Tujuan
Dokumentasi ini dibuat untuk memudahkan testing API SIMA BPJS menggunakan Swagger UI dan Postman.

## 📋 Daftar File

### 1. 🚀 [QUICK_START.md](./QUICK_START.md)
**Mulai testing dalam 5 menit!**
- Langkah-langkah cepat untuk testing
- Data testing siap pakai
- Setup Postman
- Error codes dan solusi

### 2. 📖 [DOKUMENTASI_API_SIMA_BPJS.md](./DOKUMENTASI_API_SIMA_BPJS.md)
**Dokumentasi lengkap API**
- Deskripsi semua endpoint
- Request/response examples
- Authentication flow
- Error handling
- Testing scenarios

### 3. 🔧 [SIMA_BPJS_API_POSTMAN_COLLECTION.http](./SIMA_BPJS_API_POSTMAN_COLLECTION.http)
**Collection Postman siap pakai**
- 35+ request examples
- Error testing scenarios
- Environment variables
- Step-by-step testing

### 4. 📊 [CONTOH_DATA_JSON.json](./CONTOH_DATA_JSON.json)
**Data testing dalam format JSON**
- User accounts (admin/user)
- KTP/KK data samples
- BPJS data samples
- Pembayaran data samples
- Error testing data

### 5. 🧪 [PANDUAN_TESTING.md](./PANDUAN_TESTING.md)
**Panduan testing detail**
- Persiapan testing
- Testing dengan Swagger
- Testing dengan Postman
- Skenario testing lengkap
- Troubleshooting guide

---

## 🚀 Quick Start

### Option 1: Swagger UI (Termudah)
1. Jalankan aplikasi: `dotnet run`
2. Buka: `http://localhost:5189/swagger`
3. Ikuti langkah di [QUICK_START.md](./QUICK_START.md)

### Option 2: Postman (Lebih Lengkap)
1. Import file: `SIMA_BPJS_API_POSTMAN_COLLECTION.http`
2. Setup environment
3. Ikuti langkah di [PANDUAN_TESTING.md](./PANDUAN_TESTING.md)

---

## 📊 Struktur API

### Authentication
- `GET /api/Auth/test` - Test API (no auth)
- `POST /api/Auth/register` - Register user
- `POST /api/Auth/login` - Login user

### KTP/KK Management
- `GET /api/KtpKk` - Get all KTP/KK
- `GET /api/KtpKk/{nik}` - Get by NIK
- `POST /api/KtpKk` - Create KTP/KK
- `PUT /api/KtpKk/{nik}` - Update KTP/KK
- `DELETE /api/KtpKk/{nik}` - Delete KTP/KK

### BPJS Management
- `GET /api/Bpjs` - Get all BPJS
- `GET /api/Bpjs/{id}` - Get by ID
- `POST /api/Bpjs` - Create BPJS
- `PUT /api/Bpjs/{id}` - Update BPJS
- `DELETE /api/Bpjs/{id}` - Delete BPJS
- `POST /api/Bpjs/{id}/approve` - Approve BPJS (Admin)
- `POST /api/Bpjs/{id}/deactivate` - Deactivate BPJS (Admin)

### Payment Management
- `GET /api/Pembayaran` - Get all payments
- `GET /api/Pembayaran/{id}` - Get by ID
- `POST /api/Pembayaran` - Create payment
- `PUT /api/Pembayaran/{id}` - Update payment
- `DELETE /api/Pembayaran/{id}` - Delete payment

---

## 🔐 Authentication

API menggunakan JWT Bearer Token:
1. Register/Login untuk mendapat token
2. Gunakan token di header: `Authorization: Bearer {token}`
3. Token berlaku 60 menit

### Roles
- **USER**: Akses CRUD data biasa
- **ADMIN**: Akses semua endpoint + approve/deactivate BPJS

---

## 📝 Contoh Data Testing

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

## 🧪 Testing Scenarios

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

## 🚨 Common Issues

| Issue | Solution |
|-------|----------|
| Connection refused | Pastikan aplikasi running di port 7000 |
| Unauthorized | Login dulu untuk mendapat token |
| Forbidden | Gunakan role admin untuk endpoint admin |
| Not Found | Pastikan data sudah ada |
| Bad Request | Cek format JSON dan required fields |

---

## 📞 Support

Jika mengalami masalah:
1. Cek log aplikasi di console
2. Baca troubleshooting di [PANDUAN_TESTING.md](./PANDUAN_TESTING.md)
3. Pastikan database MySQL running
4. Cek koneksi internet untuk external API

---

## 📈 Next Steps

Setelah testing selesai:
1. Implementasi di frontend
2. Integration testing
3. Performance testing
4. Security testing
5. Production deployment

---

**Happy Testing! 🎉**
