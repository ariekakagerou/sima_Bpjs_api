-- =====================================================
-- DROP TABLE - Login Activities
-- HATI-HATI! Ini akan menghapus tabel dan semua data
-- =====================================================

USE sima_bpjs;

-- Backup data terlebih dahulu (optional)
-- CREATE TABLE login_activities_backup AS SELECT * FROM login_activities;

-- Drop foreign key constraint dulu
ALTER TABLE login_activities DROP FOREIGN KEY fk_login_activities_user;

-- Drop table
DROP TABLE IF EXISTS login_activities;

-- Verifikasi tabel sudah terhapus
SHOW TABLES LIKE 'login_activities';

