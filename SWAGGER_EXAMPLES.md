# 🧪 Contoh Data untuk Swagger UI

## 🚀 Cara Menggunakan
1. Buka `http://localhost:5189/swagger`
2. Copy data dari tabel di bawah
3. Paste ke request body di Swagger
4. Klik "Execute"

---

## 1. AUTHENTICATION

### Register User
```json
{
  "username": "test_user",
  "password": "test123",
  "nik": "3201012345670001",
  "role": "USER"
}
```

### Register Admin
```json
{
  "username": "admin_bpjs",
  "password": "admin123",
  "nik": "3201012345670001",
  "role": "ADMIN"
}
```

### Login
```json
{
  "username": "test_user",
  "password": "test123"
}
```

---

## 2. KTP/KK

### Create KTP/KK - Pria
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

### Create KTP/KK - Wanita
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

### Update KTP/KK
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
  "pekerjaan": "Manager",
  "kewarganegaraan": "WNI"
}
```

---

## 3. BPJS

### Create BPJS - Kelas I
```json
{
  "nik": "3201012345670001",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### Create BPJS - Kelas II
```json
{
  "nik": "3201012345670002",
  "faskesUtama": "RS Siloam",
  "kelasPerawatan": "KELAS II",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### Create BPJS - Kelas III
```json
{
  "nik": "3201012345670003",
  "faskesUtama": "RS Cipto Mangunkusumo",
  "kelasPerawatan": "KELAS III",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### Update BPJS
```json
{
  "idBpjs": 1,
  "nik": "3201012345670001",
  "noBpjs": "1234567890123",
  "faskesUtama": "RSUD Jakarta Updated",
  "kelasPerawatan": "KELAS I",
  "statusPeserta": "PENDING",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

---

## 4. PEMBAYARAN

### Create Pembayaran - Lunas
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

### Create Pembayaran - Belum Lunas
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

### Create Pembayaran - Maret
```json
{
  "idBpjs": 1,
  "bulan": "Maret 2025",
  "tahun": 2025,
  "jumlah": 50000.00,
  "statusPembayaran": "LUNAS",
  "tanggalBayar": "2025-03-10T14:20:00.000Z"
}
```

### Update Pembayaran
```json
{
  "idPembayaran": 1,
  "idBpjs": 1,
  "bulan": "Januari 2025",
  "tahun": 2025,
  "jumlah": 55000.00,
  "statusPembayaran": "LUNAS",
  "tanggalBayar": "2025-01-15T10:30:00.000Z"
}
```

---

## 5. ERROR TESTING

### Login dengan Password Salah
```json
{
  "username": "test_user",
  "password": "wrong_password"
}
```

### Register dengan Username yang Sudah Ada
```json
{
  "username": "test_user",
  "password": "test123",
  "nik": "1234567890123456",
  "role": "USER"
}
```

### Create BPJS dengan Kelas Tidak Valid
```json
{
  "nik": "3201012345670001",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS IV",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

### Create KTP/KK dengan Field Kosong
```json
{
  "nik": "",
  "noKk": "1234567890123456",
  "namaLengkap": "John Doe",
  "tempatLahir": "Jakarta",
  "tanggalLahir": "1990-01-15T00:00:00",
  "jenisKelamin": "L",
  "alamat": "Jl. Merdeka No. 123",
  "agama": "Islam",
  "statusPerkawinan": "Belum Kawin",
  "pekerjaan": "Karyawan",
  "kewarganegaraan": "WNI"
}
```

---

## 6. TESTING FLOW

### Step 1: Test API
- `GET /api/Auth/test` (tanpa auth)

### Step 2: Register & Login
- `POST /api/Auth/register` (gunakan data di atas)
- `POST /api/Auth/login` (gunakan data di atas)
- Copy token dari response

### Step 3: Set Authorization
- Klik "Authorize" di Swagger
- Masukkan: `Bearer {token}`
- Klik "Authorize"

### Step 4: Test CRUD
- Create KTP/KK
- Get All KTP/KK
- Create BPJS
- Get All BPJS
- Create Pembayaran
- Get All Pembayaran

### Step 5: Test Admin Functions
- Login sebagai admin
- Approve BPJS: `POST /api/Bpjs/{id}/approve`
- Deactivate BPJS: `POST /api/Bpjs/{id}/deactivate`

---

## 7. VALIDATION RULES

### Kelas Perawatan BPJS
- ✅ `KELAS I`
- ✅ `KELAS II`
- ✅ `KELAS III`
- ❌ `KELAS IV` (tidak valid)

### Status Pembayaran
- ✅ `LUNAS`
- ✅ `BELUM LUNAS`

### Jenis Kelamin
- ✅ `L` (Laki-laki)
- ✅ `P` (Perempuan)

### Role User
- ✅ `USER`
- ✅ `ADMIN`

---

## 8. TIPS SWAGGER

1. **Authorization**: Set token sekali, berlaku untuk semua request
2. **Try it out**: Klik tombol ini untuk mengaktifkan form input
3. **Execute**: Klik untuk menjalankan request
4. **Response**: Lihat hasil di bagian bawah
5. **Error**: Cek status code dan message error

---

## 9. TROUBLESHOOTING

### Error 401 Unauthorized
- Pastikan sudah login dan mendapat token
- Cek format token: `Bearer {token}`
- Cek token belum expired

### Error 403 Forbidden
- Pastikan menggunakan role admin
- Endpoint approve/deactivate hanya untuk admin

### Error 400 Bad Request
- Cek format JSON
- Cek field required sudah diisi
- Cek validasi data

### Error 404 Not Found
- Cek ID yang digunakan
- Pastikan data sudah ada

---

**Happy Testing! 🎉**
