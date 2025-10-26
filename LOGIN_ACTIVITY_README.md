# Login Activity Feature - Setup Guide

## 📋 Overview

Fitur Login Activity adalah sistem tracking yang mencatat setiap aktivitas login (berhasil maupun gagal) dalam aplikasi BPJS. Fitur ini berguna untuk:

- **Security Monitoring**: Memantau percobaan login yang mencurigakan
- **Audit Trail**: Mencatat riwayat akses pengguna ke sistem
- **User Analytics**: Memahami pola penggunaan aplikasi
- **Compliance**: Memenuhi persyaratan keamanan dan audit

## 🚀 Quick Start

### 1. Database Setup

Jalankan migration script untuk membuat tabel `login_activities`:

```bash
cd database
mysql -u root -p sima_bpjs < login_activities_migration.sql
```

Atau jalankan SQL command berikut langsung:

```sql
CREATE TABLE IF NOT EXISTS `login_activities` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `user_id` INT NULL,
  `username` VARCHAR(100) NOT NULL,
  `ip_address` VARCHAR(50) NULL,
  `device` VARCHAR(200) NULL,
  `browser` VARCHAR(100) NULL,
  `status` VARCHAR(20) NOT NULL DEFAULT 'BERHASIL',
  `login_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_username_logintime` (`username`, `login_time`),
  INDEX `idx_user_id` (`user_id`),
  CONSTRAINT `fk_login_activities_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
```

### 2. Verify Database Table

```sql
-- Check if table was created
SHOW TABLES LIKE 'login_activities';

-- Check table structure
DESCRIBE login_activities;

-- Check data
SELECT * FROM login_activities ORDER BY login_time DESC LIMIT 10;
```

### 3. Backend Setup

File-file yang telah dibuat:

```
sima_Bpjs_api/
├── models/
│   ├── LoginActivity.cs              # Model entity
│   └── DTOs/
│       └── LoginActivityDto.cs       # DTOs untuk API
├── controllers/
│   ├── AuthController.cs             # (Modified) Record login
│   └── LoginActivityController.cs    # API endpoints
├── Data/
│   └── AppDbContext.cs               # (Modified) Add DbSet
├── database/
│   └── login_activities_migration.sql
└── LOGIN_ACTIVITY_API_DOCS.md        # API Documentation
```

### 4. Test Backend API

#### A. Start the API Server

```bash
cd sima_Bpjs_api
dotnet run
```

#### B. Test Login & Check Activity

1. **Login** (akan otomatis mencatat aktivitas):
```bash
curl -X POST http://localhost:5189/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrPhone": "admin_bpjs",
    "password": "admin123"
  }'
```

2. **Get Your Login Activities**:
```bash
curl -X GET "http://localhost:5189/api/LoginActivity/my-activities?limit=10" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 5. Frontend Setup

File yang dimodifikasi:

```
petugas_bpjs_kesehatan/
└── src/
    └── App.jsx    # (Modified) Added login activity UI
```

Perubahan pada frontend:
1. ✅ Added state `loginActivities` dan `loadingLoginActivities`
2. ✅ Added `useEffect` untuk fetch data saat tab pengaturan dibuka
3. ✅ Updated UI tabel aktivitas login dengan data real dari API
4. ✅ Added loading state dan empty state handling

### 6. Test Frontend

#### A. Start Frontend Dev Server

```bash
cd petugas_bpjs_kesehatan
npm run dev
```

#### B. Open Browser

1. Navigate to: `http://localhost:5173`
2. Login dengan credentials:
   - Username: `admin_bpjs`
   - Password: `admin123`
3. Klik tab **"Pengaturan"** di sidebar
4. Scroll ke bawah ke section **"Aktivitas Login Terakhir"**
5. Anda akan melihat tabel dengan data login activity real

## 📊 Features

### Automatic Login Tracking

Setiap kali user melakukan login, sistem otomatis mencatat:

- ✅ Username/Email/Phone yang digunakan
- ✅ User ID (jika login berhasil)
- ✅ IP Address client
- ✅ Device information (Windows 10, MacOS, Linux, Android, iOS)
- ✅ Browser information (Chrome, Firefox, Safari, Edge, Opera)
- ✅ Status (BERHASIL atau GAGAL)
- ✅ Timestamp login

### Failed Login Tracking

Login yang gagal juga dicatat untuk security monitoring:

```json
{
  "userId": null,
  "username": "failed_attempt@email.com",
  "ipAddress": "192.168.1.100",
  "device": "Windows 10",
  "browser": "Chrome",
  "status": "GAGAL",
  "loginTime": "2025-10-24T10:30:00"
}
```

### Device & Browser Detection

Sistem otomatis mendeteksi device dan browser dari User-Agent header:

**Device Detection:**
- Windows 10, Windows 11
- MacOS
- Linux
- Android
- iOS

**Browser Detection:**
- Chrome
- Firefox
- Safari
- Edge
- Opera

### IP Address Tracking

IP Address dideteksi dari:
1. `X-Forwarded-For` header (jika menggunakan proxy/load balancer)
2. `X-Real-IP` header
3. `RemoteIpAddress` dari HTTP context

## 🎯 Use Cases

### 1. Security Monitoring

Deteksi multiple failed login attempts:

```sql
SELECT username, COUNT(*) as failed_attempts, 
       MIN(login_time) as first_attempt,
       MAX(login_time) as last_attempt
FROM login_activities
WHERE status = 'GAGAL' 
  AND login_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR)
GROUP BY username
HAVING failed_attempts >= 3;
```

### 2. User Activity Audit

Cek siapa yang login hari ini:

```sql
SELECT DISTINCT username, device, browser, login_time
FROM login_activities
WHERE DATE(login_time) = CURDATE()
  AND status = 'BERHASIL'
ORDER BY login_time DESC;
```

### 3. Geographic Analysis

Lihat IP address yang digunakan untuk login:

```sql
SELECT username, ip_address, COUNT(*) as login_count
FROM login_activities
WHERE login_time >= DATE_SUB(NOW(), INTERVAL 7 DAY)
GROUP BY username, ip_address
ORDER BY login_count DESC;
```

### 4. Device Analytics

Statistik device yang digunakan:

```sql
SELECT device, browser, COUNT(*) as usage_count
FROM login_activities
WHERE login_time >= DATE_SUB(NOW(), INTERVAL 30 DAY)
GROUP BY device, browser
ORDER BY usage_count DESC;
```

## 🔧 Configuration

### Change Data Retention Period

Default: Data disimpan tanpa batas waktu. Untuk cleanup otomatis:

```csharp
// Di Program.cs atau background job
app.MapDelete("/api/LoginActivity/clear-old", async (AppDbContext context) =>
{
    var daysOld = 90; // Keep data for 90 days
    var cutoffDate = DateTime.Now.AddDays(-daysOld);
    var oldActivities = await context.LoginActivities
        .Where(l => l.LoginTime < cutoffDate)
        .ToListAsync();
    
    context.LoginActivities.RemoveRange(oldActivities);
    await context.SaveChangesAsync();
});
```

### Customize Device Detection

Edit `AuthController.cs`:

```csharp
private string? ParseDeviceInfo(string userAgent)
{
    if (string.IsNullOrEmpty(userAgent)) return "Unknown";
    
    // Add custom device detection logic
    if (userAgent.Contains("YourCustomDevice")) return "Custom Device";
    
    // ... existing logic
}
```

### Add Additional Tracking Fields

1. **Update Model** (`LoginActivity.cs`):
```csharp
[Column("country")]
[MaxLength(50)]
public string? Country { get; set; }
```

2. **Update DTO** (`LoginActivityDto.cs`):
```csharp
public string? Country { get; set; }
```

3. **Update Database**:
```sql
ALTER TABLE login_activities ADD COLUMN country VARCHAR(50) NULL;
```

## 📱 Frontend Integration

### Display Login Activities

Component sudah terintegrasi di `App.jsx`:

```jsx
// State
const [loginActivities, setLoginActivities] = useState([])
const [loadingLoginActivities, setLoadingLoginActivities] = useState(false)

// Fetch data
useEffect(() => {
  if (isAuthenticated && activeTab === 'pengaturan') {
    setLoadingLoginActivities(true);
    fetch('http://localhost:5189/api/LoginActivity/my-activities?limit=10', {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('bpjs_token')}`,
      },
    })
      .then(res => res.json())
      .then(data => {
        if (data?.success) {
          setLoginActivities(data.data || []);
        }
        setLoadingLoginActivities(false);
      })
      .catch(() => setLoadingLoginActivities(false));
  }
}, [isAuthenticated, activeTab]);

// Display in table
<table className="data-table">
  <thead>
    <tr>
      <th>No</th>
      <th>Tanggal & Waktu</th>
      <th>IP Address</th>
      <th>Device</th>
      <th>Browser</th>
      <th>Status</th>
    </tr>
  </thead>
  <tbody>
    {loginActivities.map((activity, index) => (
      <tr key={activity.id}>
        <td>{index + 1}</td>
        <td>{new Date(activity.loginTime).toLocaleString('id-ID')}</td>
        <td>{activity.ipAddress || 'N/A'}</td>
        <td>{activity.device || 'Unknown'}</td>
        <td>{activity.browser || 'Unknown'}</td>
        <td>
          <span className={`status ${activity.status === 'BERHASIL' ? 'active' : 'inactive'}`}>
            {activity.status}
          </span>
        </td>
      </tr>
    ))}
  </tbody>
</table>
```

## 🧪 Testing

### Unit Tests

Test file tersedia di: `LOGIN_ACTIVITY_TESTING.http`

**Basic Tests:**
```http
# 1. Get my activities
GET http://localhost:5189/api/LoginActivity/my-activities?limit=10
Authorization: Bearer YOUR_TOKEN

# 2. Get all activities (admin)
GET http://localhost:5189/api/LoginActivity?page=1&limit=10
Authorization: Bearer ADMIN_TOKEN

# 3. Get by username (admin)
GET http://localhost:5189/api/LoginActivity/user/admin_bpjs
Authorization: Bearer ADMIN_TOKEN
```

### Integration Tests

1. **Login Flow Test**:
   - Login → Check if activity recorded → Verify data

2. **Failed Login Test**:
   - Try wrong password → Check if failure recorded

3. **Multiple Logins Test**:
   - Login multiple times → Check count increases

## 🛡️ Security Considerations

### 1. Data Privacy

- IP addresses adalah data personal (GDPR compliance)
- Consider anonymizing IP after certain period
- Implement data retention policy

### 2. Access Control

- ✅ Regular users can only see their own activities
- ✅ Admin users can see all activities
- ✅ JWT authentication required for all read endpoints

### 3. Rate Limiting

Implement rate limiting untuk prevent abuse:

```csharp
// Add in Program.cs
app.UseRateLimiter();
```

### 4. Encryption

Consider encrypting IP addresses at rest:

```csharp
private string EncryptIpAddress(string ipAddress)
{
    // Implement encryption logic
}
```

## 📚 API Documentation

Full API documentation tersedia di: `LOGIN_ACTIVITY_API_DOCS.md`

**Quick Reference:**

| Endpoint | Method | Auth | Description |
|----------|--------|------|-------------|
| `/api/LoginActivity/my-activities` | GET | User/Admin | Get current user's activities |
| `/api/LoginActivity` | GET | Admin | Get all activities (paginated) |
| `/api/LoginActivity/user/{username}` | GET | Admin | Get activities by username |
| `/api/LoginActivity` | POST | None | Create activity (internal) |
| `/api/LoginActivity/{id}` | DELETE | Admin | Delete activity |
| `/api/LoginActivity/clear-old` | DELETE | Admin | Clear old activities |

## 🐛 Troubleshooting

### Issue 1: Table Not Found

**Error**: `Table 'sima_bpjs.login_activities' doesn't exist`

**Solution**:
```bash
mysql -u root -p sima_bpjs < database/login_activities_migration.sql
```

### Issue 2: 401 Unauthorized

**Error**: API returns 401 when fetching activities

**Solution**:
- Check if JWT token is valid
- Check if token is properly set in Authorization header
- Try logging in again to get fresh token

### Issue 3: Empty Data in Frontend

**Error**: Table shows "Belum ada aktivitas login"

**Solution**:
1. Login to create new activity
2. Check if API is returning data: `curl -X GET ... /my-activities`
3. Check browser console for errors
4. Verify CORS settings in backend

### Issue 4: Device/Browser Shows "Unknown"

**Cause**: User-Agent header is missing or not parseable

**Solution**:
- This is normal for API testing tools (Postman, curl)
- Real browsers will send proper User-Agent
- Can customize `ParseDeviceInfo()` and `ParseBrowserInfo()` methods

## 📞 Support

For questions or issues:
- Check `LOGIN_ACTIVITY_API_DOCS.md` for detailed API documentation
- Check `LOGIN_ACTIVITY_TESTING.http` for testing examples
- Refer to inline code comments in source files

## 🎉 Success Indicators

Your setup is successful when:

- ✅ Database table `login_activities` exists
- ✅ Login creates new record in database
- ✅ Frontend displays login activities in "Pengaturan" tab
- ✅ API endpoints return proper responses
- ✅ Both successful and failed logins are tracked

---

**Version**: 1.0.0  
**Last Updated**: October 24, 2025  
**Author**: SIMA BPJS Development Team

