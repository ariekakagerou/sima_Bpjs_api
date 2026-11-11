# 🌐 Panduan Deploy SIMA BPJS API ke Cloud (Live Swagger Publik 24/7)

Panduan ini disiapkan agar API Anda memiliki URL publik aktif (contoh: `https://sima-bpjs.up.railway.app/swagger`) sehingga **rekruter atau interviewer bisa langsung menguji API kapan saja** tanpa Anda harus menyalakan komputer.

---

## 🚀 OPSI 1: Deploy ke Railway.app (Paling Direkomendasikan, ~3 Menit)

Railway menyediakan integrasi otomatis untuk GitHub, Dockerfile, dan database MySQL dalam satu platform.

### Langkah 1: Push Perubahan Terbaru ke GitHub
Pastikan file `Dockerfile`, `.dockerignore`, dan modifikasi `Program.cs` sudah ter-push ke repository GitHub Anda:
```bash
git add .
git commit -m "feat: Add Dockerfile and Cloud auto-migration for live Swagger"
git push origin main
```

### Langkah 2: Buat Project di Railway
1. Kunjungi [railway.app](https://railway.app) dan login menggunakan akun GitHub Anda.
2. Klik tombol **"New Project"**.
3. Pilih **"Deploy from GitHub repo"** $\rightarrow$ pilih repositori `sima_Bpjs_api`.

### Langkah 3: Tambahkan Database MySQL di Railway
1. Di dalam project yang sama di dashboard Railway, klik tombol **"+ New"** (atau Add Service).
2. Pilih **"Database"** $\rightarrow$ pilih **"Add MySQL"**.
3. Railway akan membuatkan instance database MySQL dalam hitungan detik.

### Langkah 4: Hubungkan API ke MySQL & Aktifkan Swagger
1. Klik pada kartu service **`sima_Bpjs_api`**.
2. Masuk ke tab **"Variables"** $\rightarrow$ klik **"New Variable"** (atau Add Reference):
   * `ENABLE_SWAGGER` = `true`
   * `SIMA_JWT_SECRET` = `supersecretkey123456789012345678901234567890`
   * (Opsional) Jika tidak otomatis terhubung, tambahkan reference:
     `MYSQL_URL` = `${{MySQL.MYSQL_URL}}`
3. Masuk ke tab **"Settings"** $\rightarrow$ gulir ke bawah ke bagian **"Networking"** $\rightarrow$ klik **"Generate Domain"**.
   * Anda akan mendapatkan URL publik, contohnya:
     `https://sima-bpjs-api-production-xxxx.up.railway.app`

### Langkah 5: Selesai & Uji Coba!
Buka URL tersebut di browser. Backend akan:
* Otomatis membuat seluruh tabel database saat pertama kali menyala (`EnsureCreatedAsync`).
* Otomatis membuat akun **Admin** dan **User Demo**.
* Mengarahkan langsung halaman utama `/` ke `/swagger`.

---

## 🌐 OPSI 2: Deploy ke MonsterASP.NET (Khusus Windows ASP.NET & MySQL Gratis)

MonsterASP.NET adalah platform hosting gratis selamanya khusus untuk teknologi ASP.NET Core dan MySQL:

1. Daftar akun di [monsterasp.net](https://www.monsterasp.net) (Free Plan).
2. Buat **Website** baru (pilih .NET 9).
3. Buat **MySQL Database** baru di panel MonsterASP.NET.
4. Salin Connection String MySQL dari panel tersebut.
5. Publish project dari laptop Anda:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
6. Upload isi folder `./publish` via File Manager atau Web Deploy / FTP yang disediakan oleh MonsterASP.NET.
7. Di menu Environment Variables MonsterASP.NET, tambahkan:
   * `SIMA_DB_CONNECTION` = *(Connection String MySQL dari MonsterASP)*
   * `ENABLE_SWAGGER` = `true`

---

## 🔑 Kredensial Siap Pakai untuk Rekruter

Saat mencantumkan link Swagger ke rekruter, berikan kredensial berikut:

| Role | Username | Password | Keterangan |
| :--- | :--- | :--- | :--- |
| **ADMIN** | `admin_bpjs` | `Admin@BPJS2024!` | Akses Approve/Deactivate BPJS, Review Klaim, Audit Log |
| **USER** | `user_biasa` | `User@BPJS2024!` | Akses Pendaftaran BPJS, Pembayaran, Aduan |

---

## 📋 Template Ringkas untuk CV / Pesan ke Rekruter

```markdown
Project: SIMA BPJS – Management & Core Healthcare API (.NET 9, MySQL)
Repository : https://github.com/username/sima_Bpjs_api
Live Demo  : https://sima-bpjs.up.railway.app/swagger (Aktif 24/7)

Kredensial Testing:
- Admin : admin_bpjs / Admin@BPJS2024!
- User  : user_biasa / User@BPJS2024!
```

