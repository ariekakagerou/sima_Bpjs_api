# Panduan Testing API SIMA BPJS

## Daftar Isi
1. [Persiapan Testing](#persiapan-testing)
2. [Testing dengan Swagger](#testing-dengan-swagger)
3. [Testing dengan Postman](#testing-dengan-postman)
4. [Skenario Testing](#skenario-testing)
5. [Troubleshooting](#troubleshooting)

---

## Persiapan Testing

### 1. Pastikan Aplikasi Berjalan
```bash
cd sima_Bpjs_api
dotnet run
```

### 2. Akses Swagger UI
Buka browser dan akses: `http://localhost:5189/swagger`

### 3. Siapkan Data Testing
Gunakan file `CONTOH_DATA_JSON.json` sebagai referensi data untuk testing.

---

## Testing dengan Swagger

### Langkah 1: Test API Tanpa Auth
1. Buka endpoint `GET /api/Auth/test`
2. Klik "Try it out"
3. Klik "Execute"
4. Pastikan response: `{"message": "API is working", "timestamp": "..."}`

### Langkah 2: Register User
1. Buka endpoint `POST /api/Auth/register`
2. Klik "Try it out"
3. Masukkan data:
```json
{
  "username": "test_user",
  "password": "test123",
  "nik": "1234567890123456",
  "role": "USER"
}
```
4. Klik "Execute"
5. Pastikan response 201 dengan data user yang dibuat

### Langkah 3: Login
1. Buka endpoint `POST /api/Auth/login`
2. Klik "Try it out"
3. Masukkan data:
```json
{
  "username": "test_user",
  "password": "test123"
}
```
4. Klik "Execute"
5. Copy token dari response

### Langkah 4: Set Authorization
1. Klik tombol "Authorize" di bagian atas Swagger UI
2. Masukkan token dengan format: `Bearer {token}`
3. Klik "Authorize"
4. Klik "Close"

### Langkah 5: Test Endpoint yang Memerlukan Auth
Sekarang Anda bisa menguji semua endpoint yang memerlukan autentikasi.

---

## Testing dengan Postman

### 1. Import Collection
1. Buka Postman
2. Klik "Import"
3. Pilih file `SIMA_BPJS_API_POSTMAN_COLLECTION.http`
4. Collection akan ter-import

### 2. Setup Environment
1. Klik "Environments" di sidebar
2. Klik "Create Environment"
3. Beri nama: "SIMA BPJS API"
4. Tambahkan variabel:
   - `base_url`: `http://localhost:5189/api`
   - `token`: (kosong dulu)
   - `admin_token`: (kosong dulu)

### 3. Testing Step by Step

#### Step 1: Test API
```
GET {{base_url}}/Auth/test
```

#### Step 2: Register dan Login
```
POST {{base_url}}/Auth/register
POST {{base_url}}/Auth/login
```

**Setelah login, copy token dan set di environment variable `token`**

#### Step 3: Test CRUD Operations
Jalankan request secara berurutan:
1. Create KTP/KK
2. Get All KTP/KK
3. Create BPJS
4. Get All BPJS
5. Create Pembayaran
6. Get All Pembayaran

---

## Skenario Testing

### Skenario 1: Happy Path
1. ✅ Test API tanpa auth
2. ✅ Register user baru
3. ✅ Login dengan kredensial benar
4. ✅ Create KTP/KK
5. ✅ Create BPJS
6. ✅ Create Pembayaran
7. ✅ Get semua data

### Skenario 2: Error Handling
1. ✅ Login dengan password salah
2. ✅ Register dengan username yang sudah ada
3. ✅ Access endpoint tanpa token
4. ✅ Access endpoint dengan token invalid
5. ✅ Create BPJS dengan kelas tidak valid
6. ✅ Get data yang tidak ada

### Skenario 3: Role-based Access
1. ✅ Login sebagai admin
2. ✅ Approve BPJS (admin only)
3. ✅ Deactivate BPJS (admin only)
4. ✅ Login sebagai user biasa
5. ✅ Coba access endpoint admin (harus error 403)

### Skenario 4: Data Validation
1. ✅ Create KTP/KK dengan data lengkap
2. ✅ Create KTP/KK dengan field kosong
3. ✅ Create BPJS dengan NIK yang tidak ada
4. ✅ Update data dengan ID yang tidak sesuai

---

## Contoh Data Testing

### User untuk Testing
```json
{
  "admin": {
    "username": "admin_bpjs",
    "password": "admin123",
    "nik": "1234567890123456",
    "role": "ADMIN"
  },
  "user": {
    "username": "user_biasa",
    "password": "user123",
    "nik": "9876543210987654",
    "role": "USER"
  }
}
```

### KTP/KK untuk Testing
```json
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

### BPJS untuk Testing
```json
{
  "nik": "1234567890123456",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### Pembayaran untuk Testing
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

## Troubleshooting

### Error: "Connection refused"
- Pastikan aplikasi sudah running
- Cek port 7000 tidak digunakan aplikasi lain
- Cek firewall/antivirus

### Error: "Unauthorized"
- Pastikan sudah login dan mendapat token
- Cek format token: `Bearer {token}`
- Cek token belum expired (60 menit)

### Error: "Forbidden"
- Pastikan menggunakan role yang benar
- Endpoint approve/deactivate hanya untuk admin

### Error: "Not Found"
- Cek ID yang digunakan
- Pastikan data sudah ada di database

### Error: "Bad Request"
- Cek format JSON
- Cek field required sudah diisi
- Cek validasi data (contoh: kelas perawatan harus KELAS I/II/III)

### Error: "Internal Server Error"
- Cek log aplikasi
- Cek koneksi database
- Cek external API KTP

---

## Tips Testing

1. **Gunakan Environment Variables**: Set token di environment untuk kemudahan
2. **Test Berurutan**: Ikuti flow yang logis (register → login → create data)
3. **Test Error Cases**: Jangan lupa test skenario error
4. **Clean Up**: Hapus data testing setelah selesai
5. **Document Issues**: Catat bug atau issue yang ditemukan

---

## File Referensi

- `DOKUMENTASI_API_SIMA_BPJS.md` - Dokumentasi lengkap API
- `SIMA_BPJS_API_POSTMAN_COLLECTION.http` - Collection Postman
- `CONTOH_DATA_JSON.json` - Contoh data untuk testing
- `PANDUAN_TESTING.md` - File ini

---

## Support

Jika mengalami masalah dalam testing, periksa:
1. Log aplikasi di console
2. Database connection
3. Network connectivity
4. Token expiration
5. Data validation rules
