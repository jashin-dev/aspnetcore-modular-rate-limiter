using AspNetCore.ModularRateLimiter.Api.Interfaces;
using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Algorithms
{
    public class TokenBucketLimiter : IRateLimiterStrategy
    {
        private readonly TokenBucketRateLimiter _limiter;

        public TokenBucketLimiter(int tokenLimit, int tokensPerPeriod, int replenishSeconds, int queueLimit)
        {
            _limiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
            {
                TokenLimit = tokenLimit,
                TokensPerPeriod = tokensPerPeriod,
                ReplenishmentPeriod = TimeSpan.FromSeconds(replenishSeconds),
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
