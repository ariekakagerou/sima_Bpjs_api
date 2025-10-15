# Quick Start - API SIMA BPJS

## 🚀 Mulai Testing dalam 5 Menit

### 1. Jalankan Aplikasi
```bash
cd sima_Bpjs_api
dotnet run
```

### 2. Buka Swagger UI
```
http://localhost:5189/swagger
```

### 3. Test API (Tanpa Auth)
- Klik `GET /api/Auth/test`
- Klik "Try it out" → "Execute"
- ✅ Response: `{"message": "API is working"}`

### 4. Register User
- Klik `POST /api/Auth/register`
- Masukkan data:
```json
{
  "username": "test_user",
  "password": "test123",
  "nik": "1234567890123456",
  "role": "USER"
}
```

### 5. Login
- Klik `POST /api/Auth/login`
- Masukkan data:
```json
{
  "username": "test_user",
  "password": "test123"
}
```
- Copy token dari response

### 6. Set Authorization
- Klik "Authorize" (tombol hijau di atas)
- Masukkan: `Bearer {token}`
- Klik "Authorize"

### 7. Test Endpoint Lain
Sekarang Anda bisa test semua endpoint yang memerlukan auth!

---

## 📋 Data Testing Cepat

### Register Admin
```json
{
  "username": "admin",
  "password": "admin123",
  "nik": "3201012345670001",
  "role": "ADMIN"
}
```

### Register User
```json
{
  "username": "user",
  "password": "user123",
  "nik": "3201012345670002",
  "role": "USER"
}
```

### Create KTP/KK
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

### Create BPJS
```json
{
  "nik": "3201012345670001",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### Create Pembayaran
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

---

## 🔧 Postman Setup

### 1. Import Collection
- Buka Postman
- Import file: `SIMA_BPJS_API_POSTMAN_COLLECTION.http`

### 2. Create Environment
- Name: "SIMA BPJS API"
- Variables:
  - `base_url`: `http://localhost:5189/api`
  - `token`: (kosong dulu)

### 3. Test Flow
1. `GET {{base_url}}/Auth/test`
2. `POST {{base_url}}/Auth/register`
3. `POST {{base_url}}/Auth/login`
4. Set token di environment
5. Test endpoint lain

---

## ⚡ Endpoint Utama

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

## 🚨 Error Codes

| Code | Meaning | Solution |
|------|---------|----------|
| 400 | Bad Request | Cek format data |
| 401 | Unauthorized | Login dulu |
| 403 | Forbidden | Butuh role admin |
| 404 | Not Found | Data tidak ada |
| 409 | Conflict | Username sudah ada |
| 500 | Server Error | Cek log aplikasi |

---

## 📁 File Penting

- `DOKUMENTASI_API_SIMA_BPJS.md` - Dokumentasi lengkap
- `SIMA_BPJS_API_POSTMAN_COLLECTION.http` - Collection Postman
- `CONTOH_DATA_JSON.json` - Contoh data
- `PANDUAN_TESTING.md` - Panduan detail
- `QUICK_START.md` - File ini

---

## 💡 Tips

1. **Token Expired?** Login ulang untuk mendapat token baru
2. **Error 403?** Pastikan menggunakan role admin untuk endpoint admin
3. **Data Kosong?** Create data dulu sebelum get
4. **Swagger Error?** Pastikan sudah set authorization
5. **Postman Error?** Cek environment variables

---

## 🆘 Butuh Bantuan?

1. Cek log aplikasi di console
2. Baca `PANDUAN_TESTING.md` untuk detail
3. Cek `DOKUMENTASI_API_SIMA_BPJS.md` untuk referensi lengkap
4. Pastikan database MySQL sudah running
5. Cek koneksi internet untuk external API KTP
