# =====================================================
# SIMA BPJS API - Environment Variables Setup Script
# =====================================================
# This script sets up environment variables for security secrets
# Run this BEFORE starting the API

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "   SIMA BPJS API - Security Setup   " -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# ✅ JWT Secret Key
Write-Host "Setting JWT Secret Key..." -ForegroundColor Yellow
$env:SIMA_JWT_SECRET = "SuPerSecReT-JWT-KeY-F0r-BPJS-ApI-2024-M1n1mAl-32-ChArS-L0nG!"
Write-Host "✓ JWT Secret Key configured" -ForegroundColor Green

# ✅ Database Connection String
Write-Host "Setting Database Connection..." -ForegroundColor Yellow
$env:SIMA_DB_CONNECTION = "server=localhost;port=3306;database=sima_bpjs;user=root;password=;"
Write-Host "✓ Database Connection configured" -ForegroundColor Green

# ✅ Allowed CORS Origins (optional)
Write-Host "Setting CORS Origins..." -ForegroundColor Yellow
$env:ALLOWED_ORIGINS = "http://localhost:3000,http://localhost:5173"
Write-Host "✓ CORS Origins configured" -ForegroundColor Green

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "   Environment Setup Complete!      " -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Environment Variables Set:" -ForegroundColor White
Write-Host "  - SIMA_JWT_SECRET: ********" -ForegroundColor Gray
Write-Host "  - SIMA_DB_CONNECTION: server=localhost..." -ForegroundColor Gray
Write-Host "  - ALLOWED_ORIGINS: $env:ALLOWED_ORIGINS" -ForegroundColor Gray
Write-Host ""

Write-Host "⚠️  IMPORTANT NOTES:" -ForegroundColor Yellow
Write-Host "1. These variables are set for current session only" -ForegroundColor White
Write-Host "2. Run this script again if you restart PowerShell" -ForegroundColor White
Write-Host "3. For production, set these as system environment variables" -ForegroundColor White
Write-Host "4. NEVER commit sensitive values to Git" -ForegroundColor Red
Write-Host ""

Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. Install packages: dotnet restore" -ForegroundColor White
Write-Host "2. Create migration: dotnet ef migrations add AddSecurityEnhancements" -ForegroundColor White
Write-Host "3. Update database: dotnet ef database update" -ForegroundColor White
Write-Host "4. Run API: dotnet run" -ForegroundColor White
Write-Host ""

# Ask if user wants to run the API now
$response = Read-Host "Do you want to run the API now? (Y/N)"
if ($response -eq "Y" -or $response -eq "y") {
    Write-Host ""
    Write-Host "Starting SIMA BPJS API..." -ForegroundColor Cyan
    dotnet run
}

