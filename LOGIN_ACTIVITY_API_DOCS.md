# Login Activity API Documentation

## Overview
API untuk tracking dan monitoring aktivitas login pengguna dalam sistem BPJS. Endpoint ini mencatat setiap percobaan login (berhasil maupun gagal) beserta informasi device, browser, IP address, dan timestamp.

## Base URL
```
http://localhost:5189/api/LoginActivity
```

## Authentication
Semua endpoint memerlukan JWT token kecuali endpoint internal yang digunakan oleh AuthController.

Header yang diperlukan:
```
Authorization: Bearer <JWT_TOKEN>
```

---

## Endpoints

### 1. Get My Login Activities
Mendapatkan riwayat login untuk user yang sedang login.

**Endpoint:** `GET /api/LoginActivity/my-activities`

**Authorization:** Required (User atau Admin)

**Query Parameters:**
- `limit` (optional, default: 10) - Jumlah maksimum data yang ditampilkan

**Request Example:**
```http
GET http://localhost:5189/api/LoginActivity/my-activities?limit=10
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "username": "admin_bpjs",
      "ipAddress": "192.168.1.100",
      "device": "Windows 10",
      "browser": "Chrome",
      "status": "BERHASIL",
      "loginTime": "2025-10-22T14:30:15"
    },
    {
      "id": 2,
      "username": "admin_bpjs",
      "ipAddress": "192.168.1.100",
      "device": "Windows 10",
      "browser": "Chrome",
      "status": "BERHASIL",
      "loginTime": "2025-10-22T08:15:42"
    }
  ],
  "message": "Login activities retrieved successfully"
}
```

---

### 2. Get All Login Activities (Admin Only)
Mendapatkan semua riwayat login dari semua user.

**Endpoint:** `GET /api/LoginActivity`

**Authorization:** Required (Admin only)

**Query Parameters:**
- `page` (optional, default: 1) - Nomor halaman
- `limit` (optional, default: 10) - Jumlah data per halaman

**Request Example:**
```http
GET http://localhost:5189/api/LoginActivity?page=1&limit=10
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "username": "admin_bpjs",
      "ipAddress": "192.168.1.100",
      "device": "Windows 10",
      "browser": "Chrome",
      "status": "BERHASIL",
      "loginTime": "2025-10-22T14:30:15"
    }
  ],
  "total": 50,
  "page": 1,
  "limit": 10,
  "message": "Login activities retrieved successfully"
}
```

---

### 3. Get Login Activities by Username (Admin Only)
Mendapatkan riwayat login untuk username tertentu.

**Endpoint:** `GET /api/LoginActivity/user/{username}`

**Authorization:** Required (Admin only)

**Path Parameters:**
- `username` (required) - Username yang dicari

**Query Parameters:**
- `limit` (optional, default: 10) - Jumlah maksimum data

**Request Example:**
```http
GET http://localhost:5189/api/LoginActivity/user/admin_bpjs?limit=10
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "username": "admin_bpjs",
      "ipAddress": "192.168.1.100",
      "device": "Windows 10",
      "browser": "Chrome",
      "status": "BERHASIL",
      "loginTime": "2025-10-22T14:30:15"
    }
  ],
  "message": "Login activities retrieved successfully"
}
```

---

### 4. Create Login Activity (Internal)
Endpoint internal untuk mencatat aktivitas login. Biasanya dipanggil oleh AuthController.

**Endpoint:** `POST /api/LoginActivity`

**Authorization:** Not required (AllowAnonymous)

**Request Body:**
```json
{
  "userId": 1,
  "username": "admin_bpjs",
  "ipAddress": "192.168.1.100",
  "device": "Windows 10",
  "browser": "Chrome",
  "status": "BERHASIL"
}
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "username": "admin_bpjs",
    "ipAddress": "192.168.1.100",
    "device": "Windows 10",
    "browser": "Chrome",
    "status": "BERHASIL",
    "loginTime": "2025-10-22T14:30:15"
  },
  "message": "Login activity recorded"
}
```

---

### 5. Delete Login Activity (Admin Only)
Menghapus record aktivitas login tertentu.

**Endpoint:** `DELETE /api/LoginActivity/{id}`

**Authorization:** Required (Admin only)

**Path Parameters:**
- `id` (required) - ID dari login activity

**Request Example:**
```http
DELETE http://localhost:5189/api/LoginActivity/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "message": "Login activity deleted successfully"
}
```

---

### 6. Clear Old Activities (Admin Only)
Menghapus aktivitas login yang sudah lama (untuk maintenance).

**Endpoint:** `DELETE /api/LoginActivity/clear-old`

**Authorization:** Required (Admin only)

**Query Parameters:**
- `daysOld` (optional, default: 30) - Hapus data yang lebih lama dari jumlah hari ini

**Request Example:**
```http
DELETE http://localhost:5189/api/LoginActivity/clear-old?daysOld=30
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Success (200 OK):**
```json
{
  "success": true,
  "message": "Deleted 25 old login activities",
  "deletedCount": 25
}
```

---

## Data Models

### LoginActivityDto
```typescript
{
  id: number,              // ID unik dari record
  username: string,        // Username yang login
  ipAddress?: string,      // IP address client
  device?: string,         // Informasi device (OS)
  browser?: string,        // Browser yang digunakan
  status: string,          // BERHASIL atau GAGAL
  loginTime: DateTime      // Timestamp login
}
```

### LoginActivityCreateDto
```typescript
{
  userId?: number,         // ID user (null jika login gagal)
  username: string,        // Username/email/phone yang digunakan untuk login
  ipAddress?: string,      // IP address client
  device?: string,         // Informasi device
  browser?: string,        // Browser yang digunakan
  status: string           // BERHASIL atau GAGAL (default: BERHASIL)
}
```

---

## Error Responses

### 401 Unauthorized
```json
{
  "success": false,
  "message": "User not authenticated"
}
```

### 403 Forbidden
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Forbidden",
  "status": 403
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Login activity not found"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "Internal server error",
  "error": "Error details here"
}
```

---

## Integration with Frontend

### React/JavaScript Example

```javascript
// Fetch login activities for current user
const fetchMyLoginActivities = async () => {
  try {
    const response = await fetch('http://localhost:5189/api/LoginActivity/my-activities?limit=10', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('bpjs_token')}`,
      }
    });
    
    const data = await response.json();
    
    if (data.success) {
      console.log('Login activities:', data.data);
      // Update your UI with data.data
    }
  } catch (error) {
    console.error('Error fetching login activities:', error);
  }
};

// Use in React component
useEffect(() => {
  if (isAuthenticated && activeTab === 'pengaturan') {
    fetchMyLoginActivities();
  }
}, [isAuthenticated, activeTab]);
```

---

## Database Schema

```sql
CREATE TABLE `login_activities` (
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

---

## Security Considerations

1. **Role-Based Access:**
   - Regular users can only see their own login activities
   - Admin users can see all login activities

2. **Data Privacy:**
   - IP addresses are stored for security monitoring
   - Consider GDPR/privacy regulations when storing this data

3. **Automatic Cleanup:**
   - Use the `clear-old` endpoint periodically to remove old data
   - Recommended: Keep data for 90 days maximum

4. **Rate Limiting:**
   - Consider implementing rate limiting on these endpoints
   - Prevent abuse of activity logging

---

## Testing with Postman/Thunder Client

### Test 1: Get My Activities
```http
GET http://localhost:5189/api/LoginActivity/my-activities?limit=5
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```

### Test 2: Get All Activities (Admin)
```http
GET http://localhost:5189/api/LoginActivity?page=1&limit=20
Authorization: Bearer ADMIN_JWT_TOKEN_HERE
```

### Test 3: Clear Old Data (Admin)
```http
DELETE http://localhost:5189/api/LoginActivity/clear-old?daysOld=60
Authorization: Bearer ADMIN_JWT_TOKEN_HERE
```

---

## Notes

1. Login activities are automatically recorded by AuthController when user logs in
2. Both successful and failed login attempts are tracked
3. Device and browser information are parsed from User-Agent header
4. IP address is extracted from X-Forwarded-For, X-Real-IP, or RemoteIpAddress
5. Data is stored in UTC timezone

---

## Support

For issues or questions about this API, contact the development team or refer to the main API documentation.

