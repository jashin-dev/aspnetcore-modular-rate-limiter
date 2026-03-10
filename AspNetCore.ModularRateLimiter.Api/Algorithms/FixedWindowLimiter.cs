using AspNetCore.ModularRateLimiter.Api.Interfaces;
using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Algorithms
{
    public class FixedWindowLimiter : IRateLimiterStrategy
    {
        private readonly FixedWindowRateLimiter _limiter;

        public FixedWindowLimiter(int permitLimit, int windowSeconds, int queueLimit)
        {
            _limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromSeconds(windowSeconds),
                QueueLimit = queueLimit,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            });
        }

        public Task<RateLimitLease> AcquireAsync(int permits = 1)
        {
            return _limiter.AcquireAsync(permits).AsTask();
        }
    }
}
