-- =====================================================
-- INSERT SAMPLE DATA - Login Activities
-- Untuk testing tabel login_activities
-- =====================================================

USE sima_bpjs;

-- Insert 10 sample login activities
INSERT INTO login_activities (user_id, username, ip_address, device, browser, status, login_time) 
VALUES 
(1, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'BERHASIL', '2025-10-24 15:30:15'),
(1, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'BERHASIL', '2025-10-24 10:15:42'),
(1, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'BERHASIL', '2025-10-23 16:45:20'),
(1, 'admin_bpjs', '192.168.1.101', 'Windows 10', 'Firefox', 'BERHASIL', '2025-10-23 09:30:00'),
(1, 'admin_bpjs', '192.168.1.102', 'MacOS', 'Safari', 'BERHASIL', '2025-10-22 14:20:10'),
(NULL, 'wrong_user', '192.168.1.200', 'Windows 10', 'Chrome', 'GAGAL', '2025-10-24 11:00:00'),
(NULL, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'GAGAL', '2025-10-23 08:00:00'),
(1, 'admin_bpjs', '192.168.1.100', 'Android', 'Chrome', 'BERHASIL', '2025-10-22 07:30:00'),
(1, 'admin_bpjs', '192.168.1.100', 'iOS', 'Safari', 'BERHASIL', '2025-10-21 18:45:30'),
(1, 'admin_bpjs', '192.168.1.100', 'Linux', 'Firefox', 'BERHASIL', '2025-10-21 12:15:00');

-- Verifikasi data
SELECT * FROM login_activities ORDER BY login_time DESC;

-- Lihat statistik
SELECT 
    status, 
    COUNT(*) as total,
    COUNT(DISTINCT username) as unique_users
FROM login_activities 
GROUP BY status;

