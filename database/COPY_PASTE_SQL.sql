-- =====================================================
-- COPY & PASTE SQL - Login Activities
-- Salin semua query di bawah ini ke MySQL console
-- =====================================================

USE sima_bpjs;

CREATE TABLE IF NOT EXISTS login_activities (
  id INT NOT NULL AUTO_INCREMENT,
  user_id INT NULL,
  username VARCHAR(100) NOT NULL,
  ip_address VARCHAR(50) NULL,
  device VARCHAR(200) NULL,
  browser VARCHAR(100) NULL,
  status VARCHAR(20) NOT NULL DEFAULT 'BERHASIL',
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

ALTER TABLE login_activities COMMENT = 'Stores user login activity history for security monitoring';

