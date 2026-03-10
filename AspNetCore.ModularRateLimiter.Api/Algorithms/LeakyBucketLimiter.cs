using AspNetCore.ModularRateLimiter.Api.Interfaces;
using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Algorithms
{
    public class LeakyBucketLimiter : IRateLimiterStrategy
    {
        private readonly ConcurrencyLimiter _limiter;

        public LeakyBucketLimiter(int permitLimit, int queueLimit)
        {
            _limiter = new ConcurrencyLimiter(new ConcurrencyLimiterOptions
            {
                PermitLimit = permitLimit,
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
