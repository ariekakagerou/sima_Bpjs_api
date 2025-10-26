-- =====================================================
-- VERIFY TABLE - Login Activities
-- Query untuk memverifikasi tabel sudah dibuat
-- =====================================================

USE sima_bpjs;

-- 1. Cek apakah tabel ada
SHOW TABLES LIKE 'login_activities';

-- 2. Lihat struktur tabel
DESCRIBE login_activities;

-- 3. Lihat indexes
SHOW INDEX FROM login_activities;

-- 4. Lihat foreign keys
SELECT 
    CONSTRAINT_NAME,
    TABLE_NAME,
    COLUMN_NAME,
    REFERENCED_TABLE_NAME,
    REFERENCED_COLUMN_NAME
FROM
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE
    TABLE_SCHEMA = 'sima_bpjs'
    AND TABLE_NAME = 'login_activities'
    AND REFERENCED_TABLE_NAME IS NOT NULL;

-- 5. Lihat isi tabel (10 data terakhir)
SELECT * FROM login_activities 
ORDER BY login_time DESC 
LIMIT 10;

-- 6. Hitung jumlah data
SELECT COUNT(*) as total_records FROM login_activities;

-- 7. Statistik per status
SELECT 
    status,
    COUNT(*) as jumlah,
    MIN(login_time) as login_pertama,
    MAX(login_time) as login_terakhir
FROM login_activities
GROUP BY status;

