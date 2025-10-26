# Setup Login Activity Feature
# PowerShell script untuk Windows

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  SIMA BPJS - Login Activity Setup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Database configuration
$dbHost = "localhost"
$dbUser = "root"
$dbPassword = "123"
$dbName = "sima_bpjs"

Write-Host "🔧 Konfigurasi Database:" -ForegroundColor Yellow
Write-Host "   Host: $dbHost"
Write-Host "   User: $dbUser"
Write-Host "   Database: $dbName"
Write-Host ""

# Check if MySQL is available
Write-Host "📋 Step 1: Checking MySQL connection..." -ForegroundColor Green

try {
    $mysqlPath = Get-Command mysql -ErrorAction Stop
    Write-Host "✅ MySQL ditemukan di: $($mysqlPath.Source)" -ForegroundColor Green
} catch {
    Write-Host "❌ MySQL tidak ditemukan. Pastikan MySQL sudah terinstall." -ForegroundColor Red
    Write-Host "   Tambahkan MySQL ke PATH atau install terlebih dahulu." -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "📋 Step 2: Creating login_activities table..." -ForegroundColor Green

# SQL command
$sqlCommand = @"
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
"@

# Run SQL command
try {
    $sqlCommand | mysql -h $dbHost -u $dbUser -p$dbPassword $dbName 2>&1 | Out-Null
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Tabel login_activities berhasil dibuat!" -ForegroundColor Green
    } else {
        Write-Host "⚠️ Tabel mungkin sudah ada atau terjadi error kecil (check manual)" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Error saat membuat tabel: $_" -ForegroundColor Red
    Write-Host "   Coba jalankan SQL secara manual atau periksa kredensial database" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "📋 Step 3: Verifying table structure..." -ForegroundColor Green

# Verify table
try {
    $verifyCommand = "DESCRIBE login_activities;"
    $result = $verifyCommand | mysql -h $dbHost -u $dbUser -p$dbPassword $dbName 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Tabel berhasil diverifikasi!" -ForegroundColor Green
        Write-Host ""
        Write-Host "📊 Struktur Tabel:" -ForegroundColor Cyan
        Write-Host $result
    } else {
        Write-Host "⚠️ Tidak dapat memverifikasi tabel" -ForegroundColor Yellow
    }
} catch {
    Write-Host "⚠️ Error saat verifikasi: $_" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📚 Next Steps:" -ForegroundColor Yellow
Write-Host "   1. Jalankan backend API: dotnet run" -ForegroundColor White
Write-Host "   2. Jalankan frontend: npm run dev" -ForegroundColor White
Write-Host "   3. Login ke aplikasi untuk test" -ForegroundColor White
Write-Host "   4. Buka tab 'Pengaturan' untuk melihat aktivitas login" -ForegroundColor White
Write-Host ""
Write-Host "📖 Documentation:" -ForegroundColor Yellow
Write-Host "   - Setup Guide: LOGIN_ACTIVITY_README.md" -ForegroundColor White
Write-Host "   - API Docs: LOGIN_ACTIVITY_API_DOCS.md" -ForegroundColor White
Write-Host "   - Testing: LOGIN_ACTIVITY_TESTING.http" -ForegroundColor White
Write-Host ""

# Optional: Insert sample data
$insertSample = Read-Host "Mau insert sample data untuk testing? (y/n)"

if ($insertSample -eq 'y' -or $insertSample -eq 'Y') {
    Write-Host ""
    Write-Host "📋 Inserting sample data..." -ForegroundColor Green
    
    $sampleData = @"
INSERT INTO login_activities (user_id, username, ip_address, device, browser, status, login_time) VALUES 
(1, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'BERHASIL', '2025-10-22 14:30:15'),
(1, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'BERHASIL', '2025-10-22 08:15:42'),
(1, 'admin_bpjs', '192.168.1.100', 'Windows 10', 'Chrome', 'BERHASIL', '2025-10-21 16:45:20');
"@
    
    try {
        $sampleData | mysql -h $dbHost -u $dbUser -p$dbPassword $dbName 2>&1 | Out-Null
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Sample data berhasil diinsert!" -ForegroundColor Green
            
            # Show data
            $showData = "SELECT * FROM login_activities ORDER BY login_time DESC LIMIT 5;"
            $dataResult = $showData | mysql -h $dbHost -u $dbUser -p$dbPassword $dbName 2>&1
            Write-Host ""
            Write-Host "📊 Sample Data:" -ForegroundColor Cyan
            Write-Host $dataResult
        } else {
            Write-Host "⚠️ Gagal insert sample data (mungkin user_id tidak ada)" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "⚠️ Error saat insert sample data: $_" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "🎉 All Done! Selamat menggunakan fitur Login Activity!" -ForegroundColor Green
Write-Host ""

# Keep window open
Read-Host "Press Enter to exit"

