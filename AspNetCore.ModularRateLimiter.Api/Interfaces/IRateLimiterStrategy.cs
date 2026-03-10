using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Interfaces
{
    public interface IRateLimiterStrategy
    {
        Task<RateLimitLease> AcquireAsync(int permits = 1);
    }
}
