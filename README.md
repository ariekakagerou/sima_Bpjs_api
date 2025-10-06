# 🏥 API SIMA BPJS - Dokumentasi Testing

## 🎯 Tujuan
Dokumentasi lengkap untuk testing API SIMA BPJS menggunakan Swagger UI dan Postman.

---

## 🚀 MULAI TESTING SEKARANG

### Option 1: Swagger UI (Termudah)
```
1. Jalankan: dotnet run
2. Buka: http://localhost:5189/swagger
3. Baca: QUICK_START.md
4. Gunakan: SWAGGER_EXAMPLES.md
```

### Option 2: Postman (Lebih Lengkap)
```
1. Import: SIMA_BPJS_API_POSTMAN_COLLECTION.http
2. Baca: PANDUAN_TESTING.md
3. Setup environment
4. Test endpoint
```

---

## 📚 DOKUMENTASI LENGKAP

### 🚀 **QUICK START**
- [QUICK_START.md](./QUICK_START.md) - Mulai testing dalam 5 menit
- [SWAGGER_EXAMPLES.md](./SWAGGER_EXAMPLES.md) - Contoh data untuk Swagger UI

### 📖 **DOKUMENTASI LENGKAP**
- [DOKUMENTASI_API_SIMA_BPJS.md](./DOKUMENTASI_API_SIMA_BPJS.md) - Dokumentasi lengkap API
- [PANDUAN_TESTING.md](./PANDUAN_TESTING.md) - Panduan testing detail

### 🔧 **FILE TESTING**
- [SIMA_BPJS_API_POSTMAN_COLLECTION.http](./SIMA_BPJS_API_POSTMAN_COLLECTION.http) - Collection Postman
- [CONTOH_DATA_JSON.json](./CONTOH_DATA_JSON.json) - Data testing JSON

### 📊 **NAVIGASI**
- [README_TESTING.md](./README_TESTING.md) - Index utama dokumentasi
- [INDEX_DOKUMENTASI.md](./INDEX_DOKUMENTASI.md) - Overview semua file
- [RINGKASAN_AKHIR.md](./RINGKASAN_AKHIR.md) - Ringkasan lengkap

---

## 📊 ENDPOINT UTAMA

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/Auth/test` | ❌ | Test API |
| POST | `/Auth/register` | ❌ | Register user |
| POST | `/Auth/login` | ❌ | Login user |
| GET | `/KtpKk` | ✅ | Get all KTP/KK |
| POST | `/KtpKk` | ✅ | Create KTP/KK |
| GET | `/Bpjs` | ✅ | Get all BPJS |
| POST | `/Bpjs` | ✅ | Create BPJS |
| POST | `/Bpjs/{id}/approve` | ✅ Admin | Approve BPJS |
| GET | `/Pembayaran` | ✅ | Get all Pembayaran |
| POST | `/Pembayaran` | ✅ | Create Pembayaran |

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

### Register Admin
```json
{
  "username": "admin_bpjs",
  "password": "admin123",
  "nik": "1234567890123456",
  "role": "ADMIN"
}
```

### Register User
```json
{
  "username": "user_biasa",
  "password": "user123",
  "email": "user@email.com",
  "phoneNumber": "08123456789",
  "dateOfBirth": "2000-01-01",
  "nik": "9876543210987654"
}
```

### Create BPJS
```json
{
  "nik": "1234567890123456",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

---

## 🧪 TESTING FLOW

### Step 1: Test API
- `GET /api/Auth/test` (tanpa auth)

### Step 2: Register & Login
- `POST /api/Auth/register`
- `POST /api/Auth/login`
- Copy token

### Step 3: Set Authorization
- Swagger: Klik "Authorize" → `Bearer {token}`
- Postman: Set di environment variable

### Step 4: Test CRUD
- Create KTP/KK
- Create BPJS
- Create Pembayaran
- Test semua GET endpoints

### Step 5: Test Admin
- Login sebagai admin
- Test approve/deactivate BPJS

---

## 🚨 ERROR CODES

| Code | Meaning | Solution |
|------|---------|----------|
| 400 | Bad Request | Cek format data |
| 401 | Unauthorized | Login dulu |
| 403 | Forbidden | Butuh role admin |
| 404 | Not Found | Data tidak ada |
| 409 | Conflict | Username sudah ada |
| 500 | Server Error | Cek log aplikasi |

---

## 📁 FILE PENTING

### Untuk Pemula
- `QUICK_START.md` - Mulai testing
- `SWAGGER_EXAMPLES.md` - Contoh data Swagger

### Untuk Developer
- `DOKUMENTASI_API_SIMA_BPJS.md` - Referensi lengkap
- `SIMA_BPJS_API_POSTMAN_COLLECTION.http` - Collection Postman

### Untuk Testing Mendalam
- `PANDUAN_TESTING.md` - Panduan detail
- `CONTOH_DATA_JSON.json` - Data testing

---

## 🎯 REKOMENDASI

### Mulai dengan Swagger UI
1. Baca `QUICK_START.md`
2. Gunakan `SWAGGER_EXAMPLES.md`
3. Test endpoint satu per satu

### Lanjut ke Postman
1. Import `SIMA_BPJS_API_POSTMAN_COLLECTION.http`
2. Baca `PANDUAN_TESTING.md`
3. Test semua skenario

### Referensi Lengkap
1. Baca `DOKUMENTASI_API_SIMA_BPJS.md`
2. Gunakan `CONTOH_DATA_JSON.json`
3. Test error handling

---

## 🚀 LANGKAH SELANJUTNYA

### Setelah Testing Selesai
1. Implementasi di frontend
2. Integration testing
3. Performance testing
4. Security testing
5. Production deployment

### Maintenance
1. Update dokumentasi jika ada perubahan API
2. Tambah contoh data baru
3. Update testing scenarios
4. Monitor error logs

---

## 📞 SUPPORT

### Jika Mengalami Masalah
1. **Cek log aplikasi** di console
2. **Baca troubleshooting** di `PANDUAN_TESTING.md`
3. **Pastikan database** MySQL running
4. **Cek koneksi internet** untuk external API

### File Bantuan
- `PANDUAN_TESTING.md` - Troubleshooting detail
- `DOKUMENTASI_API_SIMA_BPJS.md` - Referensi lengkap
- `QUICK_START.md` - Solusi cepat

---

## 🎉 SELAMAT TESTING!

Dokumentasi ini sudah lengkap dengan:
- ✅ 8 file dokumentasi
- ✅ 35+ contoh request
- ✅ 50+ contoh data
- ✅ 5 skenario testing
- ✅ 10+ error cases
- ✅ Panduan Swagger UI
- ✅ Collection Postman
- ✅ Troubleshooting guide

**Pilih file yang sesuai dengan kebutuhan Anda dan mulai testing!**

---

*Dokumentasi dibuat untuk memudahkan testing API SIMA BPJS. Semua file sudah siap digunakan untuk testing dengan Swagger UI dan Postman.*
