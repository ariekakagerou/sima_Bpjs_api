-- Fix Database Schema: Add Missing Security Columns
-- Run this in phpMyAdmin or MySQL command line

USE sima_bpjs;

-- Add failed_login_attempts column to users table
ALTER TABLE `users` 
ADD COLUMN `failed_login_attempts` INT NOT NULL DEFAULT 0 
AFTER `password_hash`;

-- Add lockout_end column to users table
ALTER TABLE `users` 
ADD COLUMN `lockout_end` DATETIME NULL 
AFTER `failed_login_attempts`;

-- Create refresh_tokens table if not exists
CREATE TABLE IF NOT EXISTS `refresh_tokens` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NOT NULL,
  `token` VARCHAR(500) NOT NULL,
  `expires_at` DATETIME NOT NULL,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `is_revoked` TINYINT(1) NOT NULL DEFAULT 0,
  `revoked_at` DATETIME NULL,
  `created_by_ip` VARCHAR(50) NULL,
  `created_by_device` VARCHAR(200) NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UK_refresh_tokens_token` (`token`),
  KEY `IX_refresh_tokens_user_id_is_revoked` (`user_id`, `is_revoked`),
  CONSTRAINT `FK_refresh_tokens_users_user_id` 
    FOREIGN KEY (`user_id`) 
    REFERENCES `users` (`id`) 
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Verify changes
SELECT 'Schema updated successfully!' AS Status;
SHOW COLUMNS FROM `users`;
SHOW TABLES;

