# ASP.NET Core Modular Rate Limiter

A flexible, modular ASP.NET Core rate limiting middleware with multiple pluggable algorithms and dynamic, configuration-based limiter selection. This project demonstrates production-ready rate limiting with modern ASP.NET Core patterns and best practices.

![.NET Version](https://img.shields.io/badge/.NET-10.0-blue)
![License](https://img.shields.io/badge/License-MIT-green)

## Table of Contents

- [Features](#features)
- [Rate Limiting Algorithms](#rate-limiting-algorithms)
  - [Fixed Window](#fixed-window)
  - [Sliding Window](#sliding-window)
  - [Token Bucket](#token-bucket)
  - [Leaky Bucket](#leaky-bucket)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
  - [Clone the Repository](#clone-the-repository)
  - [Restore Dependencies](#restore-dependencies)
  - [Run the Application](#run-the-application)
- [Configuration Guide](#configuration-guide)
- [API Usage](#api-usage)
- [Testing](#testing)
- [Architecture](#architecture)
- [License](#license)

## Features

**Multiple Rate Limiting Algorithms** - Choose from four different strategies to match your use case

**Dynamic Algorithm Selection** - Switch between algorithms via configuration without code changes

**Configuration-Based** - Full configuration through `appsettings.json`

**Modular Design** - Easy to extend with new algorithms

**Queue Management** - Built-in request queuing with configurable queue limits

**ASP.NET Core 10** - Built with the latest .NET framework

**OpenAPI/Swagger Documentation** - Interactive API documentation included

## Rate Limiting Algorithms

### Fixed Window

**How it works:** Divides time into fixed-size windows (e.g., 60-second intervals). A request is allowed only if the total number of requests in the current window hasn't exceeded the limit.

**Characteristics:**
- Simple to understand and implement
- Low memory overhead
- Can have "burst" behavior at window boundaries
- Best for: Simple throttling with fixed intervals

**Configuration:**
```json
{
  "RateLimiting": {
    "Algorithm": "FixedWindow",
    "PermitLimit": 100,        // Max requests per window
    "WindowSeconds": 60,        // Window size in seconds
    "QueueLimit": 10           // Max queued requests
  }
}
```

### Sliding Window

**How it works:** Maintains a continuously moving time window. Only requests within the last N seconds are counted, ensuring a smoother rate limit compared to fixed windows.

**Characteristics:**
- Smoother rate limiting than fixed windows
- Eliminates burst behavior at boundaries
- Slightly higher memory usage than fixed window
- More granular distribution of requests
- Best for: Applications requiring consistent request distribution

**Configuration:**
```json
{
  "RateLimiting": {
    "Algorithm": "SlidingWindow",
    "PermitLimit": 100,         // Max requests per window
    "WindowSeconds": 60,         // Window size in seconds
    "SegmentsPerWindow": 6,     // Number of segments for precision
    "QueueLimit": 10            // Max queued requests
  }
}
```

> **Note:** More segments = better accuracy but slightly higher memory usage. Typically 4-10 segments is optimal.

### Token Bucket

**How it works:** Tokens are added to a bucket at a fixed rate. Each request consumes one token. If tokens are available, the request is allowed; otherwise, it waits or is rejected.

**Characteristics:**
- Allows controlled bursts up to the bucket size
- Supports varying token consumption rates
- Better for handling traffic spikes
- Commonly used in cloud APIs (AWS, Google Cloud)
- Best for: APIs with bursty traffic patterns, bandwidth throttling

**Configuration:**
```json
{
  "RateLimiting": {
    "Algorithm": "TokenBucket",
    "TokenLimit": 100,              // Total bucket capacity
    "TokensPerPeriod": 10,          // Tokens added per period
    "ReplenishmentPeriodSeconds": 1, // How often to add tokens
    "QueueLimit": 10                // Max queued requests
  }
}
```

**Example:** With `TokenLimit: 100` and `TokensPerPeriod: 10` per second:
- Sustains 10 requests/second
- Can handle bursts up to 100 requests
- Replenishes at 10 tokens/second

### Leaky Bucket

**How it works:** Requests are placed in a queue (bucket) and processed at a constant rate. When the queue is full, new requests are rejected.

**Characteristics:**
- Ensures smooth, constant output rate
- Prevents sudden traffic spikes
- Predictable request processing
- Queue-based, first-in-first-out (FIFO) processing
- Best for: Smooth rate limiting with fixed output rate, preventing thundering herd

**Configuration:**
```json
{
  "RateLimiting": {
    "Algorithm": "LeakyBucket",
    "PermitLimit": 50,    // Queue capacity / concurrent request limit
    "QueueLimit": 10      // Max additional queued requests
  }
}
```

## Project Structure

```
aspnetcore-modular-rate-limiter/
├── README.md                          # This file
├── LICENSE                            # MIT License
│
└── src/
    ├── rate-limit-test.js             # JavaScript test file for API testing
    │
    └── AspNetCore.ModularRateLimiter.Api/
        │
        ├── Program.cs                 # Application startup and configuration
        ├── appsettings.json           # Configuration settings
        ├── appsettings.Development.json
        │
        ├── Algorithms/                # Rate limiting algorithm implementations
        │   ├── FixedWindowLimiter.cs
        │   ├── SlidingWindowLimiter.cs
        │   ├── TokenBucketLimiter.cs
        │   └── LeakyBucketLimiter.cs
        │
        ├── Configuration/             # Configuration models
        │   └── RateLimitingOptions.cs # Rate limiting options/settings class
        │
        ├── Controllers/               # API controllers
        │   └── TestController.cs      # Test endpoint
        │
        ├── Extensions/                # Extension methods
        │   ├── ServiceCollectionExtensions.cs # DI setup
        │   └── ApplicationBuilderExtensions.cs # Middleware setup
        │
        ├── Factory/                   # Factory pattern implementations
        │   └── RateLimiterFactory.cs  # Creates appropriate limiter based on config
        │
        ├── Interfaces/                # Abstractions
        │   └── IRateLimiterStrategy.cs # Rate limiter interface
        │
        ├── Middleware/                # Custom middleware
        │   └── RateLimitingMiddleware.cs # Core rate limiting logic
        │
        ├── Models/                    # Data models
        │   └── ApiResponse.cs         # API response wrapper
        │
        └── Utils/                     # Utility classes
            ├── Constants/
            │   └── RateLimiterConstants.cs
            └── Helpers/
                └── ResponseWriter.cs  # Response formatting helpers
```

## Prerequisites

- **Operating System:** Windows, macOS, or Linux
- **.NET SDK:** Version 10.0 or higher
- **Visual Studio / VS Code:** (Optional, but recommended)
  - Visual Studio 2024 (with ASP.NET and web development workload)
  - OR Visual Studio Code with C# Dev Kit extension

## Installation & Setup

### Clone the Repository

```bash
git clone https://github.com/yourusername/aspnetcore-modular-rate-limiter.git
cd aspnetcore-modular-rate-limiter
```

### Restore Dependencies

```bash
cd src/AspNetCore.ModularRateLimiter.Api
dotnet restore
```

### Run the Application

**Option 1: Using dotnet CLI**
```bash
dotnet run
```

**Option 2: Using Visual Studio**
1. Open `AspNetCore.ModularRateLimiter.Api.slnx` in Visual Studio
2. Press `F5` or click the "Run" button
3. The application will start and open in your default browser

**Option 3: Using VS Code**
1. Install the C# Dev Kit extension
2. Open the workspace folder
3. Press `F5` to launch the debugger

The application will start on: `https://localhost:5001` (or the configured port)

## Configuration Guide

### Basic Configuration

Edit `appsettings.json` in the project root:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "RateLimiting": {
    "Algorithm": "LeakyBucket",              // Algorithm to use
    "PermitLimit": 50,                       // Primary limit (depends on algorithm)
    "WindowSeconds": 60,                     // Time window in seconds
    "SegmentsPerWindow": 6,                  // For Sliding Window only
    "TokenLimit": 100,                       // For Token Bucket only
    "TokensPerPeriod": 10,                   // For Token Bucket only
    "ReplenishmentPeriodSeconds": 1,         // For Token Bucket only
    "QueueLimit": 10                         // Max queued requests
  }
}
```

### Development Configuration

For development with relaxed limits, use `appsettings.Development.json`:

```json
{
  "RateLimiting": {
    "Algorithm": "TokenBucket",
    "TokenLimit": 1000,
    "TokensPerPeriod": 100,
    "ReplenishmentPeriodSeconds": 1,
    "QueueLimit": 100
  }
}
```

### Selecting the Right Algorithm

| Algorithm | Use Case | Pros | Cons |
|-----------|----------|------|------|
| **Fixed Window** | Simple APIs, rate limiting by hour/day | Simple, low overhead | Burst at boundaries |
| **Sliding Window** | APIs needing smooth distribution | No burst behavior, accurate | Slightly more memory |
| **Token Bucket** | APIs with bursty traffic, file uploads | Handles spikes, flexible | More complex |
| **Leaky Bucket** | Smooth output rate, prevent spikes | Consistent rate, fair queuing | No burst capacity |

## API Usage

### Test Endpoint

The application includes a test endpoint to check rate limiting:

**Endpoint:** `GET /api/test`

**Request:**
```bash
curl https://localhost:5001/api/test
```

**Response (Success - 200 OK):**
```json
{
  "success": true,
  "message": "Request processed successfully",
  "timestamp": "2024-03-12T10:30:45.123Z",
  "path": "/api/test"
}
```

**Response (Rate Limited - 429 Too Many Requests):**
```json
{
  "success": false,
  "message": "Rate limit exceeded. Try again after X seconds.",
  "timestamp": "2024-03-12T10:30:46.456Z",
  "retryAfter": 5
}
```

### Headers

The response includes rate limiting information:
- `Retry-After`: Seconds to wait before retrying (included with 429 responses)

## Testing

### Using the Provided Test Script

A JavaScript test file is included (`src/rate-limit-test.js`) to verify the rate limiting behavior:

```bash
node src/rate-limit-test.js
```

### Manual Testing with cURL

**Test Fixed Window Limiting (10 requests in 60 seconds):**
```bash
# Test with rapid requests
for i in {1..15}; do
  curl -i https://localhost:5001/api/test
  echo "\nRequest $i"
done
```

**Test with Delays:**
```bash
# Space out requests
for i in {1..5}; do
  curl https://localhost:5001/api/test
  sleep 5  # Wait 5 seconds between requests
done
```

### Using Postman

1. Import the API into Postman
2. Create a Collection with multiple requests to `/api/test`
3. Use the "Runner" feature to send rapid requests
4. Observe the 429 responses when limits are exceeded

## Architecture

### Design Patterns Used

1. **Strategy Pattern** - Multiple algorithm implementations via `IRateLimiterStrategy`
2. **Factory Pattern** - `RateLimiterFactory` creates appropriate limiters
3. **Dependency Injection** - Configuration and services injected via built-in DI
4. **Middleware Pattern** - `RateLimitingMiddleware` intercepts requests
5. **Options Pattern** - Configuration via `RateLimitingOptions`

### Request Flow

```
HTTP Request
    |
RateLimitingMiddleware
    |
RateLimiterFactory (selects algorithm)
    |
IRateLimiterStrategy Implementation
    ├─ FixedWindowLimiter
    ├─ SlidingWindowLimiter
    ├─ TokenBucketLimiter
    └─ LeakyBucketLimiter
    |
AcquireAsync() - Attempt to acquire permit
    |
    ├─ Permit granted → Continue to controller
    └─ Limit exceeded → 429 Too Many Requests
```

### Extending with New Algorithms

To add a new rate limiting algorithm:

1. **Create a new class** implementing `IRateLimiterStrategy`:
```csharp
public class MyCustomLimiter : IRateLimiterStrategy
{
    public Task<RateLimitLease> AcquireAsync(int permits = 1)
    {
        // Your implementation
    }

    public int GetRetryAfterSeconds()
    {
        // Return seconds to retry
    }
}
```

2. **Update the factory** in `RateLimiterFactory.cs` to handle your algorithm
3. **Add configuration options** to `RateLimitingOptions.cs`
4. **Update `appsettings.json`** with your new algorithm name

## Performance Considerations

- **Fixed Window:** O(1) memory and time - best for scale
- **Sliding Window:** O(segments) memory - trade-off accuracy for memory  
- **Token Bucket:** O(1) memory - efficient for burst handling
- **Leaky Bucket:** O(queue size) memory - depends on queue configuration

For high-traffic scenarios (>10k req/s), consider:
- Using Fixed Window or Token Bucket
- Increasing `SegmentsPerWindow` for Sliding Window to ≤4
- Reducing `QueueLimit` to prevent memory bloat

## Troubleshooting

**All requests return 429 Too Many Requests?**
- Check `PermitLimit` is not set too low
- Verify algorithm is correctly spelled in configuration
- Review `QueueLimit` - might be too small

**Rate limiting not working?**
- Ensure middleware is registered in `Program.cs`
- Check that routes match the `/api` prefix pattern
- Verify configuration is loaded: check application logs

**High memory usage?**
- Reduce `QueueLimit` 
- For Sliding Window, reduce `SegmentsPerWindow`
- Check if requests are accumulating in the queue

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Built with:** ASP.NET Core 10, C#, and modern .NET practices

For issues or contributions, please submit a GitHub issue or pull request.
