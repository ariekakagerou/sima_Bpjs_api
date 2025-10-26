-- =====================================================
-- Migration Script: Login Activities Table
-- Database: sima_bpjs
-- Created: 2025-10-24
-- Purpose: Track user login activities for security
-- =====================================================

-- Gunakan database sima_bpjs
USE sima_bpjs;

-- Buat tabel login_activities
CREATE TABLE IF NOT EXISTS login_activities (
  id INT NOT NULL AUTO_INCREMENT,
  user_id INT NULL,
  username VARCHAR(100) NOT NULL,
  ip_address VARCHAR(50) NULL,
  device VARCHAR(200) NULL,
  browser VARCHAR(100) NULL,
  status VARCHAR(20) NOT NULL DEFAULT 'BERHASIL' COMMENT 'BERHASIL, GAGAL',
  login_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  INDEX idx_username_logintime (username, login_time),
  INDEX idx_user_id (user_id),
  CONSTRAINT fk_login_activities_user
    FOREIGN KEY (user_id)
    REFERENCES users (id)
    ON DELETE SET NULL
    ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Tambahkan comment ke tabel
ALTER TABLE login_activities COMMENT = 'Stores user login activity history for security monitoring';

-- =====================================================
-- SELESAI! Tabel berhasil dibuat.
-- =====================================================

-- Untuk verifikasi, jalankan query berikut:
-- DESCRIBE login_activities;
-- SELECT * FROM login_activities LIMIT 10;

