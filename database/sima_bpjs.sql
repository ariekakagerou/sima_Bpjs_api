-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 24 Sep 2025 pada 04.11
-- Versi server: 10.4.32-MariaDB
-- Versi PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sima_bpjs`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `admin`
--

CREATE TABLE `admin` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktur dari tabel `bpjs`
--

CREATE TABLE `bpjs` (
  `id_bpjs` int(11) NOT NULL,
  `nik` varchar(16) DEFAULT NULL,
  `no_bpjs` varchar(20) DEFAULT NULL,
  `faskes_utama` varchar(100) DEFAULT NULL,
  `kelas_perawatan` enum('KELAS I','KELAS II','KELAS III') DEFAULT NULL,
  `status_peserta` enum('PENDING','AKTIF','NONAKTIF') DEFAULT 'PENDING',
  `tanggal_daftar` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `bpjs`
--

INSERT INTO `bpjs` (`id_bpjs`, `nik`, `no_bpjs`, `faskes_utama`, `kelas_perawatan`, `status_peserta`, `tanggal_daftar`) VALUES
(3, '3201012345670001', '0001234567890', 'RS Umum Daerah', 'KELAS I', 'AKTIF', '2024-01-15'),
(4, '3201012345670002', '4787525670002', 'RS Siloam Bandung', 'KELAS II', 'AKTIF', '2025-01-27');

-- --------------------------------------------------------

--
-- Struktur dari tabel `ktp_kk`
--

CREATE TABLE `ktp_kk` (
  `nik` varchar(16) NOT NULL,
  `no_kk` varchar(16) NOT NULL,
  `nama_lengkap` varchar(100) NOT NULL,
  `tempat_lahir` varchar(50) DEFAULT NULL,
  `tanggal_lahir` date DEFAULT NULL,
  `jenis_kelamin` enum('L','P') DEFAULT NULL,
  `alamat` text DEFAULT NULL,
  `agama` varchar(50) DEFAULT NULL,
  `status_perkawinan` varchar(50) DEFAULT NULL,
  `pekerjaan` varchar(100) DEFAULT NULL,
  `kewarganegaraan` varchar(50) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `ktp_kk`
--

INSERT INTO `ktp_kk` (`nik`, `no_kk`, `nama_lengkap`, `tempat_lahir`, `tanggal_lahir`, `jenis_kelamin`, `alamat`, `agama`, `status_perkawinan`, `pekerjaan`, `kewarganegaraan`, `created_at`) VALUES
('3201012345670001', '0000000000000000', 'Ahmad Wijaya', 'Jakarta', '1990-01-15', 'L', 'Jl. Merdeka No. 123, RT 001, RW 005', 'Islam', 'Kawin', 'Pegawai Swasta', 'WNI', '2025-09-23 11:49:20'),
('3201012345670002', '0000000000000000', 'Siti Nurhaliza', 'Bandung', '1992-03-20', 'P', 'Jl. Sudirman No. 456, RT 002, RW 003', 'Islam', 'Belum Kawin', 'Guru', 'WNI', '2025-09-24 01:47:07');

-- --------------------------------------------------------

--
-- Struktur dari tabel `pembayaran_bpjs`
--

CREATE TABLE `pembayaran_bpjs` (
  `id_pembayaran` int(11) NOT NULL,
  `id_bpjs` int(11) DEFAULT NULL,
  `bulan` varchar(20) DEFAULT NULL,
  `tahun` year(4) DEFAULT NULL,
  `jumlah` decimal(15,2) DEFAULT NULL,
  `status_pembayaran` enum('LUNAS','BELUM LUNAS') DEFAULT NULL,
  `tanggal_bayar` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struktur dari tabel `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(100) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `role` varchar(20) NOT NULL DEFAULT 'USER',
  `nik` varchar(16) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `users`
--

INSERT INTO `users` (`id`, `username`, `password_hash`, `role`, `nik`, `created_at`) VALUES
(1, 'string', 'Afs2CwzrZq2tkYi0faN7+UsTQB9/ezsQXcmSpAKBg3hIYhUVo0E2s0S9wOfgNkW4KA==', 'STRING', 'string', '2025-09-23 05:42:54'),
(2, 'test_user', 'AcPaZvcY8B8v7SlNA/Xjs8P7qPrje8qn1L/vOt84y59Focko9Fl3FNCKchBwt4lowg==', 'USER', '1234567890123456', '2025-09-23 18:23:34'),
(3, 'admin_bpjs', 'ATqz3H3AsWUhz28FuCvkDdJDv8EdkKKPngfJg0Su2ACM+gD6/1Qh4wiG0SJnRSnBqw==', 'ADMIN', '3201012345670001', '2025-09-23 19:04:08');

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `admin`
--
ALTER TABLE `admin`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `bpjs`
--
ALTER TABLE `bpjs`
  ADD PRIMARY KEY (`id_bpjs`),
  ADD UNIQUE KEY `no_bpjs` (`no_bpjs`),
  ADD KEY `nik` (`nik`);

--
-- Indeks untuk tabel `ktp_kk`
--
ALTER TABLE `ktp_kk`
  ADD PRIMARY KEY (`nik`);

--
-- Indeks untuk tabel `pembayaran_bpjs`
--
ALTER TABLE `pembayaran_bpjs`
  ADD PRIMARY KEY (`id_pembayaran`),
  ADD KEY `id_bpjs` (`id_bpjs`);

--
-- Indeks untuk tabel `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ux_users_username` (`username`),
  ADD KEY `ix_users_nik` (`nik`);

--
-- AUTO_INCREMENT untuk tabel yang dibuang
--

--
-- AUTO_INCREMENT untuk tabel `admin`
--
ALTER TABLE `admin`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT untuk tabel `bpjs`
--
ALTER TABLE `bpjs`
  MODIFY `id_bpjs` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT untuk tabel `pembayaran_bpjs`
--
ALTER TABLE `pembayaran_bpjs`
  MODIFY `id_pembayaran` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT untuk tabel `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Ketidakleluasaan untuk tabel pelimpahan (Dumped Tables)
--

--
-- Ketidakleluasaan untuk tabel `bpjs`
--
ALTER TABLE `bpjs`
  ADD CONSTRAINT `bpjs_ibfk_1` FOREIGN KEY (`nik`) REFERENCES `ktp_kk` (`nik`);

--
-- Ketidakleluasaan untuk tabel `pembayaran_bpjs`
--
ALTER TABLE `pembayaran_bpjs`
  ADD CONSTRAINT `pembayaran_bpjs_ibfk_1` FOREIGN KEY (`id_bpjs`) REFERENCES `bpjs` (`id_bpjs`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
