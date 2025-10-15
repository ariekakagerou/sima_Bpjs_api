# SIMA BPJS API - CRUD Performance Testing

## Overview
Dokumen ini berisi panduan untuk melakukan performance testing pada operasi CRUD SIMA BPJS API.

## Test Environment
- **Framework**: .NET 9.0
- **Database**: MySQL/MariaDB
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Token

## Performance Metrics

### Target Performance
- **Response Time**: < 200ms untuk operasi CRUD sederhana
- **Throughput**: > 100 requests/second
- **Memory Usage**: < 500MB untuk 1000 concurrent users
- **Database Connection**: < 50ms untuk query sederhana

### Test Scenarios

#### 1. Single Record Operations
```bash
# Create single KTP/KK record
POST /api/KtpKk
Response Time Target: < 100ms

# Read single KTP/KK record
GET /api/KtpKk/{nik}
Response Time Target: < 50ms

# Update single KTP/KK record
PUT /api/KtpKk/{nik}
Response Time Target: < 100ms

# Delete single KTP/KK record
DELETE /api/KtpKk/{nik}
Response Time Target: < 100ms
```

#### 2. Bulk Operations
```bash
# Create multiple records (10 records)
POST /api/KtpKk (10 times)
Response Time Target: < 1000ms total

# Read all records
GET /api/KtpKk
Response Time Target: < 200ms

# Update multiple records (10 records)
PUT /api/KtpKk/{nik} (10 times)
Response Time Target: < 1000ms total
```

#### 3. Complex Queries
```bash
# Get BPJS with related KTP/KK data
GET /api/Bpjs
Response Time Target: < 300ms

# Get payments with related BPJS and KTP/KK data
GET /api/Pembayaran
Response Time Target: < 400ms
```

## Load Testing Tools

### 1. Apache JMeter
```xml
<!-- JMeter Test Plan untuk CRUD Operations -->
<TestPlan>
  <ThreadGroup>
    <elementProp name="ThreadGroup.main_controller" elementType="LoopController">
      <stringProp name="LoopController.loops">100</stringProp>
    </elementProp>
    <stringProp name="ThreadGroup.num_threads">10</stringProp>
    <stringProp name="ThreadGroup.ramp_time">10</stringProp>
  </ThreadGroup>
  
  <HTTPSamplerProxy>
    <stringProp name="HTTPSampler.domain">localhost</stringProp>
    <stringProp name="HTTPSampler.port">7000</stringProp>
    <stringProp name="HTTPSampler.path">/api/KtpKk</stringProp>
    <stringProp name="HTTPSampler.method">GET</stringProp>
  </HTTPSamplerProxy>
</TestPlan>
```

### 2. Artillery.io
```yaml
# artillery-config.yml
config:
  target: 'https://localhost:7000'
  phases:
    - duration: 60
      arrivalRate: 10
  http:
    timeout: 30

scenarios:
  - name: "CRUD Operations"
    weight: 100
    flow:
      - get:
          url: "/api/KtpKk"
          headers:
            Authorization: "Bearer {{token}}"
      - post:
          url: "/api/KtpKk"
          headers:
            Authorization: "Bearer {{token}}"
            Content-Type: "application/json"
          json:
            nik: "{{$randomString(16)}}"
            noKk: "{{$randomString(16)}}"
            namaLengkap: "Test User {{$randomString(10)}}"
```

### 3. Postman Collection Runner
```json
{
  "info": {
    "name": "SIMA BPJS CRUD Performance Test",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Create KTP/KK",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Authorization",
            "value": "Bearer {{token}}"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"nik\": \"{{$randomString(16)}}\",\n  \"noKk\": \"{{$randomString(16)}}\",\n  \"namaLengkap\": \"Test User {{$randomString(10)}}\"\n}"
        },
        "url": {
          "raw": "{{baseUrl}}/api/KtpKk",
          "host": ["{{baseUrl}}"],
          "path": ["api", "KtpKk"]
        }
      }
    }
  ]
}
```

## Database Performance Optimization

### 1. Indexing Strategy
```sql
-- Primary indexes
CREATE INDEX idx_ktp_kk_nik ON ktp_kk(nik);
CREATE INDEX idx_bpjs_id ON bpjs(id_bpjs);
CREATE INDEX idx_bpjs_nik ON bpjs(nik);
CREATE INDEX idx_pembayaran_id ON pembayaran_bpjs(id_pembayaran);
CREATE INDEX idx_pembayaran_bpjs_id ON pembayaran_bpjs(id_bpjs);
CREATE INDEX idx_users_id ON users(id);
CREATE INDEX idx_users_username ON users(username);

-- Composite indexes for common queries
CREATE INDEX idx_bpjs_status_nik ON bpjs(status_peserta, nik);
CREATE INDEX idx_pembayaran_status_tahun ON pembayaran_bpjs(status_pembayaran, tahun);
CREATE INDEX idx_users_role_created ON users(role, created_at);
```

### 2. Query Optimization
```csharp
// Optimized query with proper includes
var bpjsList = await _context.Bpjs
    .Include(b => b.KtpKk)
    .Where(b => b.StatusPeserta == "AKTIF")
    .Select(b => new BpjsResponseDto
    {
        IdBpjs = b.IdBpjs,
        Nik = b.Nik,
        NoBpjs = b.NoBpjs,
        FaskesUtama = b.FaskesUtama,
        KelasPerawatan = b.KelasPerawatan,
        StatusPeserta = b.StatusPeserta,
        TanggalDaftar = b.TanggalDaftar,
        KtpKk = b.KtpKk != null ? new KtpKkResponseDto
        {
            Nik = b.KtpKk.Nik,
            NamaLengkap = b.KtpKk.NamaLengkap,
            // Only select needed fields
        } : null
    })
    .ToListAsync();
```

### 3. Connection Pooling
```csharp
// In Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }
    );
}, ServiceLifetime.Scoped);
```

## Monitoring and Profiling

### 1. Application Insights
```csharp
// Add to Program.cs
builder.Services.AddApplicationInsightsTelemetry();

// Custom telemetry
public class BpjsController : ControllerBase
{
    private readonly TelemetryClient _telemetryClient;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BpjsResponseDto>>> GetAll()
    {
        using var activity = _telemetryClient.StartActivity("GetAllBpjs");
        try
        {
            // Implementation
        }
        catch (Exception ex)
        {
            _telemetryClient.TrackException(ex);
            throw;
        }
    }
}
```

### 2. Performance Counters
```csharp
// Custom performance counters
public class PerformanceService
{
    private readonly Counter _requestCounter;
    private readonly Histogram _responseTimeHistogram;
    
    public PerformanceService()
    {
        _requestCounter = Meter.CreateCounter<int>("api_requests_total");
        _responseTimeHistogram = Meter.CreateHistogram<double>("api_response_time_seconds");
    }
    
    public void RecordRequest(string endpoint, double responseTime)
    {
        _requestCounter.Add(1, new KeyValuePair<string, object>("endpoint", endpoint));
        _responseTimeHistogram.Record(responseTime, new KeyValuePair<string, object>("endpoint", endpoint));
    }
}
```

## Test Results Analysis

### 1. Response Time Analysis
```bash
# Calculate average response time
awk '{sum+=$2; count++} END {print "Average:", sum/count}' response_times.log

# Calculate 95th percentile
sort -n response_times.log | awk 'BEGIN{i=0} {s[i]=$2; i++} END {print s[int(NR*0.95)]}'
```

### 2. Throughput Analysis
```bash
# Calculate requests per second
grep "200 OK" access.log | wc -l | awk '{print $1/60 " requests/second"}'
```

### 3. Error Rate Analysis
```bash
# Calculate error rate
total_requests=$(wc -l < access.log)
error_requests=$(grep -v "200 OK" access.log | wc -l)
error_rate=$(echo "scale=2; $error_requests * 100 / $total_requests" | bc)
echo "Error rate: $error_rate%"
```

## Performance Recommendations

### 1. Caching Strategy
```csharp
// Add Redis caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

// Cache frequently accessed data
[HttpGet]
public async Task<ActionResult<IEnumerable<BpjsResponseDto>>> GetAll()
{
    var cacheKey = "bpjs_all";
    var cachedData = await _cache.GetAsync<IEnumerable<BpjsResponseDto>>(cacheKey);
    
    if (cachedData != null)
    {
        return Ok(new { success = true, data = cachedData, message = "Data retrieved from cache" });
    }
    
    var data = await _context.Bpjs.Include(b => b.KtpKk).ToListAsync();
    await _cache.SetAsync(cacheKey, data, TimeSpan.FromMinutes(5));
    
    return Ok(new { success = true, data = data, message = "Data retrieved from database" });
}
```

### 2. Pagination
```csharp
// Add pagination to list endpoints
[HttpGet]
public async Task<ActionResult<PagedResult<BpjsResponseDto>>> GetAll(
    int page = 1, 
    int pageSize = 10,
    string search = null)
{
    var query = _context.Bpjs.Include(b => b.KtpKk).AsQueryable();
    
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(b => b.NoBpjs.Contains(search) || 
                                b.KtpKk.NamaLengkap.Contains(search));
    }
    
    var totalCount = await query.CountAsync();
    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(b => new BpjsResponseDto
        {
            // Mapping
        })
        .ToListAsync();
    
    var result = new PagedResult<BpjsResponseDto>
    {
        Items = items,
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
    };
    
    return Ok(new { success = true, data = result, message = "Data retrieved successfully" });
}
```

### 3. Async/Await Best Practices
```csharp
// Use async/await properly
public async Task<ActionResult<BpjsResponseDto>> GetById(int id)
{
    // Use async methods
    var bpjs = await _context.Bpjs
        .Include(b => b.KtpKk)
        .FirstOrDefaultAsync(b => b.IdBpjs == id);
    
    // Use ConfigureAwait(false) for library code
    await SomeAsyncMethod().ConfigureAwait(false);
    
    return Ok(new { success = true, data = bpjs, message = "Data retrieved successfully" });
}
```

## Conclusion
Performance testing adalah bagian penting dari pengembangan API. Dengan mengikuti panduan ini, Anda dapat memastikan bahwa SIMA BPJS API berkinerja optimal dan dapat menangani beban kerja yang diharapkan.
