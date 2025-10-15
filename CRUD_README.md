# SIMA BPJS API - CRUD Operations

## Overview
Sistem Informasi Manajemen Administrasi BPJS (SIMA BPJS) API menyediakan operasi CRUD (Create, Read, Update, Delete) lengkap untuk mengelola data administrasi BPJS.

## Features
- ✅ **Complete CRUD Operations** untuk semua entitas
- ✅ **Data Validation** dengan business rules
- ✅ **Cascade Delete Protection** untuk menjaga integritas data
- ✅ **JWT Authentication** dengan role-based access
- ✅ **Comprehensive Error Handling**
- ✅ **RESTful API Design**
- ✅ **Swagger Documentation**

## Database Schema

### Tables
1. **admin** - Data administrator sistem
2. **ktp_kk** - Data kependudukan (KTP/KK)
3. **bpjs** - Data peserta BPJS
4. **pembayaran_bpjs** - Data pembayaran BPJS
5. **users** - Data pengguna sistem

### Relationships
- `ktp_kk` (1) → `bpjs` (many)
- `bpjs` (1) → `pembayaran_bpjs` (many)
- `users` (1) → `bpjs` (many) via NIK

## API Endpoints

### Authentication
- `POST /api/Auth/login` - Login user
- `POST /api/Auth/register` - Register user baru

### Admin Management
- `GET /api/Admin` - Get all admins (Admin only)
- `GET /api/Admin/{id}` - Get admin by ID (Admin only)
- `POST /api/Admin` - Create admin (Admin only)
- `PUT /api/Admin/{id}` - Update admin (Admin only)
- `DELETE /api/Admin/{id}` - Delete admin (Admin only)

### KTP/KK Management
- `GET /api/KtpKk` - Get all KTP/KK data
- `GET /api/KtpKk/{nik}` - Get KTP/KK by NIK
- `POST /api/KtpKk` - Create KTP/KK data
- `PUT /api/KtpKk/{nik}` - Update KTP/KK data
- `DELETE /api/KtpKk/{nik}` - Delete KTP/KK data

### BPJS Management
- `GET /api/Bpjs` - Get all BPJS data
- `GET /api/Bpjs/{id}` - Get BPJS by ID
- `POST /api/Bpjs` - Create BPJS data
- `PUT /api/Bpjs/{id}` - Update BPJS data
- `DELETE /api/Bpjs/{id}` - Delete BPJS data
- `POST /api/Bpjs/{id}/approve` - Approve participant (Admin only)
- `POST /api/Bpjs/{id}/deactivate` - Deactivate participant (Admin only)

### Payment Management
- `GET /api/Pembayaran` - Get all payments
- `GET /api/Pembayaran/{id}` - Get payment by ID
- `POST /api/Pembayaran` - Create payment
- `PUT /api/Pembayaran/{id}` - Update payment
- `DELETE /api/Pembayaran/{id}` - Delete payment

### User Management
- `GET /api/User` - Get all users (Admin only)
- `GET /api/User/{id}` - Get user by ID
- `POST /api/User` - Create user (Admin only)
- `PUT /api/User/{id}` - Update user
- `PUT /api/User/{id}/password` - Update password
- `DELETE /api/User/{id}` - Delete user (Admin only)

## Data Models

### Admin
```csharp
public class Admin
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Password { get; set; }
}
```

### KTP/KK
```csharp
public class KtpKk
{
    public string Nik { get; set; }
    public string NoKk { get; set; }
    public string NamaLengkap { get; set; }
    public string TempatLahir { get; set; }
    public DateTime? TanggalLahir { get; set; }
    public string JenisKelamin { get; set; }
    public string Alamat { get; set; }
    public string Agama { get; set; }
    public string StatusPerkawinan { get; set; }
    public string Pekerjaan { get; set; }
    public string Kewarganegaraan { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

### BPJS
```csharp
public class Bpjs
{
    public int IdBpjs { get; set; }
    public string Nik { get; set; }
    public string NoBpjs { get; set; }
    public string FaskesUtama { get; set; }
    public string KelasPerawatan { get; set; }
    public string StatusPeserta { get; set; }
    public DateTime? TanggalDaftar { get; set; }
}
```

### Pembayaran BPJS
```csharp
public class PembayaranBpjs
{
    public int IdPembayaran { get; set; }
    public int IdBpjs { get; set; }
    public string Bulan { get; set; }
    public int Tahun { get; set; }
    public decimal Jumlah { get; set; }
    public string StatusPembayaran { get; set; }
    public DateTime? TanggalBayar { get; set; }
}
```

### User
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public string Nik { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## Business Rules

### 1. Cascade Delete Protection
- **KTP/KK** cannot be deleted if it has related BPJS records
- **BPJS** cannot be deleted if it has related payment records
- **User** cannot be deleted if it has related BPJS records

### 2. Unique Constraints
- **Username** must be unique across all users
- **Email** must be unique (if provided)
- **Phone Number** must be unique (if provided)
- **NIK** must be unique (if provided)
- **No BPJS** must be unique

### 3. Data Validation
- **NIK**: Exactly 16 characters
- **No KK**: Exactly 16 characters
- **Kelas Perawatan**: KELAS I, KELAS II, or KELAS III
- **Status Peserta**: PENDING, AKTIF, or NONAKTIF
- **Status Pembayaran**: LUNAS or BELUM LUNAS
- **Role**: USER or ADMIN

### 4. Authorization Rules
- **Admin operations** require ADMIN role
- **Users** can only update their own data (except admins)
- **Password updates** require current password verification

## Error Handling

### HTTP Status Codes
- `200 OK` - Success
- `201 Created` - Resource created successfully
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource already exists
- `500 Internal Server Error` - Server error

### Error Response Format
```json
{
  "success": false,
  "message": "Error description",
  "errors": {
    "fieldName": ["Validation error message"]
  }
}
```

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- MySQL/MariaDB database
- Visual Studio Code or Visual Studio

### Installation
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run database migrations
4. Start the application

### Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sima_bpjs;Uid=root;Pwd=password;"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "SIMA-BPJS-API",
    "Audience": "SIMA-BPJS-Client"
  }
}
```

### Testing
Use the provided `CRUD_TESTING_EXAMPLES.http` file to test all endpoints.

## API Documentation
- Swagger UI: `https://localhost:7000/swagger`
- Complete documentation: `API_CRUD_DOCUMENTATION.md`

## Security Features
- JWT-based authentication
- Password hashing with salt
- Role-based authorization
- Input validation and sanitization
- SQL injection protection via Entity Framework

## Performance Considerations
- Entity Framework with MySQL
- Optimized queries with proper indexing
- Lazy loading for related entities
- Connection pooling

## Contributing
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License
This project is licensed under the MIT License.
