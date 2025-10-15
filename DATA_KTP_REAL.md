# 📋 Data KTP Real dari Endpoint Eksternal

## 🔗 Endpoint KTP Eksternal
**URL:** [https://ktp.chasouluix.biz.id/api/ktp/all](https://ktp.chasouluix.biz.id/api/ktp/all)

## 📊 Data KTP yang Tersedia

### 1. Ahmad Wijaya
```json
{
  "nik": "3201012345670001",
  "nama_lengkap": "Ahmad Wijaya",
  "tempat_lahir": "Jakarta",
  "tanggal_lahir": "1990-01-15",
  "jenis_kelamin": "L",
  "golongan_darah": "A",
  "agama": "Islam",
  "status_perkawinan": "Kawin",
  "pekerjaan": "Pegawai Swasta",
  "kewarganegaraan": "WNI",
  "alamat": "Jl. Merdeka No. 123, RT 001, RW 005",
  "provinsi": "DKI Jakarta",
  "kabupaten": "Jakarta Pusat",
  "kecamatan": "Gambir",
  "kelurahan": "Gambir",
  "rt": "001",
  "rw": "005",
  "kode_pos": "10110",
  "no_telepon": "081234567890",
  "status": "selesai"
}
```

### 2. Siti Nurhaliza
```json
{
  "nik": "3201012345670002",
  "nama_lengkap": "Siti Nurhaliza",
  "tempat_lahir": "Bandung",
  "tanggal_lahir": "1992-03-20",
  "jenis_kelamin": "P",
  "golongan_darah": "B",
  "agama": "Islam",
  "status_perkawinan": "Belum Kawin",
  "pekerjaan": "Guru",
  "kewarganegaraan": "WNI",
  "alamat": "Jl. Sudirman No. 456, RT 002, RW 003",
  "provinsi": "Jawa Barat",
  "kabupaten": "Bandung",
  "kecamatan": "Bandung Wetan",
  "kelurahan": "Cihapit",
  "rt": "002",
  "rw": "003",
  "kode_pos": "40114",
  "no_telepon": "081234567891",
  "status": "proses_cetak"
}
```

### 3. Budi Santoso
```json
{
  "nik": "3201012345670003",
  "nama_lengkap": "Budi Santoso",
  "tempat_lahir": "Surabaya",
  "tanggal_lahir": "1988-07-10",
  "jenis_kelamin": "L",
  "golongan_darah": "O",
  "agama": "Kristen",
  "status_perkawinan": "Kawin",
  "pekerjaan": "Wiraswasta",
  "kewarganegaraan": "WNI",
  "alamat": "Jl. Pahlawan No. 789, RT 003, RW 007",
  "provinsi": "Jawa Timur",
  "kabupaten": "Surabaya",
  "kecamatan": "Gubeng",
  "kelurahan": "Gubeng",
  "rt": "003",
  "rw": "007",
  "kode_pos": "60281",
  "no_telepon": "081234567892",
  "status": "verifikasi"
}
```

### 4. Dewi Lestari
```json
{
  "nik": "3201012345670004",
  "nama_lengkap": "Dewi Lestari",
  "tempat_lahir": "Yogyakarta",
  "tanggal_lahir": "1995-11-25",
  "jenis_kelamin": "P",
  "golongan_darah": "AB",
  "agama": "Hindu",
  "status_perkawinan": "Belum Kawin",
  "pekerjaan": "Mahasiswa",
  "kewarganegaraan": "WNI",
  "alamat": "Jl. Malioboro No. 321, RT 004, RW 002",
  "provinsi": "D.I. Yogyakarta",
  "kabupaten": "Yogyakarta",
  "kecamatan": "Malioboro",
  "kelurahan": "Sosromenduran",
  "rt": "004",
  "rw": "002",
  "kode_pos": "55271",
  "no_telepon": "081234567893",
  "status": "pending"
}
```

### 5. Rizki Pratama
```json
{
  "nik": "3201012345670005",
  "nama_lengkap": "Rizki Pratama",
  "tempat_lahir": "Medan",
  "tanggal_lahir": "1993-09-05",
  "jenis_kelamin": "L",
  "golongan_darah": "A",
  "agama": "Islam",
  "status_perkawinan": "Cerai Hidup",
  "pekerjaan": "Pegawai Negeri Sipil",
  "kewarganegaraan": "WNI",
  "alamat": "Jl. Gatot Subroto No. 654, RT 005, RW 001",
  "provinsi": "Sumatera Utara",
  "kabupaten": "Medan",
  "kecamatan": "Medan Baru",
  "kelurahan": "Babura",
  "rt": "005",
  "rw": "001",
  "kode_pos": "20154",
  "no_telepon": "081234567894",
  "status": "ditolak"
}
```

---

## 🔄 Mapping ke Model KTP/KK API

### Field Mapping
| Endpoint KTP | Model KTP/KK | Keterangan |
|--------------|--------------|------------|
| `nik` | `nik` | NIK (16 digit) |
| `nama_lengkap` | `namaLengkap` | Nama lengkap |
| `tempat_lahir` | `tempatLahir` | Tempat lahir |
| `tanggal_lahir` | `tanggalLahir` | Tanggal lahir |
| `jenis_kelamin` | `jenisKelamin` | L/P |
| `alamat` | `alamat` | Alamat lengkap |
| `agama` | `agama` | Agama |
| `status_perkawinan` | `statusPerkawinan` | Status perkawinan |
| `pekerjaan` | `pekerjaan` | Pekerjaan |
| `kewarganegaraan` | `kewarganegaraan` | WNI/WNA |

### Field yang Tidak Ada di Endpoint
- `noKk` - Akan diisi dengan NIK yang sama
- `createdAt` - Akan diisi otomatis

---

## 🧪 Contoh Penggunaan untuk Testing

### 1. Register User dengan NIK Real
```json
{
  "username": "ahmad_wijaya",
  "password": "password123",
  "nik": "3201012345670001",
  "role": "USER"
}
```

### 2. Create KTP/KK dengan Data Real
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

### 3. Create BPJS dengan NIK Real
```json
{
  "nik": "3201012345670001",
  "faskesUtama": "RSUD Jakarta",
  "kelasPerawatan": "KELAS I",
  "tanggalDaftar": "2025-01-27T00:00:00"
}
```

---

## ⚠️ Catatan Penting

### 1. Validasi NIK
- Gunakan NIK yang tersedia di endpoint untuk testing yang valid
- NIK harus 16 digit
- Format: `3201012345670001` - `3201012345670005`

### 2. Status KTP
- `selesai` - KTP sudah selesai
- `proses_cetak` - Sedang dicetak
- `verifikasi` - Sedang diverifikasi
- `pending` - Menunggu proses
- `ditolak` - Ditolak

### 3. Testing Flow
1. **Register User** dengan NIK yang tersedia
2. **Create KTP/KK** dengan data yang sesuai
3. **Create BPJS** dengan NIK yang sama
4. **Test CRUD operations**

---

## 🔗 Integrasi dengan API

### Auto-sync KTP Data
Ketika membuat BPJS dengan NIK yang belum ada di database lokal, API akan:
1. Cek endpoint eksternal: `https://ktp.chasouluix.biz.id/api/ktp/all`
2. Cari NIK yang sesuai
3. Auto-create data KTP/KK di database lokal
4. Lanjutkan proses pembuatan BPJS

### Error Handling
- Jika NIK tidak ditemukan di endpoint eksternal → Error 400
- Jika endpoint eksternal tidak accessible → Error 400
- Jika data tidak valid → Error 400

---

## 📝 Daftar NIK untuk Testing

| NIK | Nama | Status | Keterangan |
|-----|------|--------|------------|
| `3201012345670001` | Ahmad Wijaya | selesai | ✅ Ready untuk testing |
| `3201012345670002` | Siti Nurhaliza | proses_cetak | ✅ Ready untuk testing |
| `3201012345670003` | Budi Santoso | verifikasi | ✅ Ready untuk testing |
| `3201012345670004` | Dewi Lestari | pending | ✅ Ready untuk testing |
| `3201012345670005` | Rizki Pratama | ditolak | ⚠️ Status ditolak |

---

**Gunakan NIK yang tersedia di endpoint ini untuk testing yang valid dan real! 🎯**
