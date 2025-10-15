-- Migration script untuk menambahkan tabel admin jika belum ada
-- Jalankan script ini jika tabel admin belum ada di database

-- Cek apakah tabel admin sudah ada
CREATE TABLE IF NOT EXISTS `admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Insert data admin default jika belum ada
INSERT IGNORE INTO `admin` (`id`, `username`, `password`) VALUES
(1, 'admin_bpjs', 123456);

-- Hapus constraint yang mungkin konflik terlebih dahulu
ALTER TABLE `bpjs` DROP FOREIGN KEY IF EXISTS `bpjs_ibfk_1`;
ALTER TABLE `pembayaran_bpjs` DROP FOREIGN KEY IF EXISTS `pembayaran_bpjs_ibfk_1`;

-- Update foreign key untuk tabel bpjs
ALTER TABLE `bpjs` 
ADD CONSTRAINT `bpjs_ibfk_1` FOREIGN KEY (`nik`) REFERENCES `ktp_kk` (`nik`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- Update foreign key untuk tabel pembayaran_bpjs
ALTER TABLE `pembayaran_bpjs` 
ADD CONSTRAINT `pembayaran_bpjs_ibfk_1` FOREIGN KEY (`id_bpjs`) REFERENCES `bpjs` (`id_bpjs`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- Pastikan semua index sudah ada (gunakan CREATE INDEX IF NOT EXISTS)
CREATE INDEX IF NOT EXISTS `ix_users_username` ON `users` (`username`);
CREATE INDEX IF NOT EXISTS `ix_users_email` ON `users` (`email`);
CREATE INDEX IF NOT EXISTS `ix_users_phone_number` ON `users` (`phone_number`);
CREATE INDEX IF NOT EXISTS `ix_users_nik` ON `users` (`nik`);

-- Pastikan unique constraints sudah ada (gunakan ALTER TABLE dengan IF NOT EXISTS)
-- Hapus constraint yang mungkin konflik terlebih dahulu
ALTER TABLE `users` DROP INDEX IF EXISTS `ux_users_username`;
ALTER TABLE `users` DROP INDEX IF EXISTS `ux_users_email`;
ALTER TABLE `users` DROP INDEX IF EXISTS `ux_users_phone_number`;

-- Tambahkan unique constraints
ALTER TABLE `users` ADD CONSTRAINT `ux_users_username` UNIQUE (`username`);
ALTER TABLE `users` ADD CONSTRAINT `ux_users_email` UNIQUE (`email`);
ALTER TABLE `users` ADD CONSTRAINT `ux_users_phone_number` UNIQUE (`phone_number`);

-- Pastikan auto increment sudah benar
ALTER TABLE `admin` AUTO_INCREMENT = 1;
ALTER TABLE `bpjs` AUTO_INCREMENT = 6;
ALTER TABLE `pembayaran_bpjs` AUTO_INCREMENT = 1;
ALTER TABLE `users` AUTO_INCREMENT = 6;
