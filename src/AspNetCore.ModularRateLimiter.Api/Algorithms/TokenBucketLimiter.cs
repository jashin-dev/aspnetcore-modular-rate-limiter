using AspNetCore.ModularRateLimiter.Api.Interfaces;
using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Algorithms
{
    public class TokenBucketLimiter : IRateLimiterStrategy
    {
        private readonly TokenBucketRateLimiter _limiter;
        private readonly TimeSpan _replenishmentPeriod;
        private readonly DateTime _startTime = DateTime.UtcNow;

        public TokenBucketLimiter(int tokenLimit, int tokensPerPeriod, int replenishSeconds, int queueLimit)
        {
            _replenishmentPeriod = TimeSpan.FromSeconds(replenishSeconds);
            _limiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
            {
                TokenLimit = tokenLimit,
                TokensPerPeriod = tokensPerPeriod,
                ReplenishmentPeriod = _replenishmentPeriod,
                QueueLimit = queueLimit,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            });
        }

        public Task<RateLimitLease> AcquireAsync(int permits = 1)
        {
            return _limiter.AcquireAsync(permits).AsTask();
        }

        public int GetRetryAfterSeconds()
        {
            var now = DateTime.UtcNow;
            var elapsed = (now - _startTime).TotalSeconds;
            var elapsedInPeriod = elapsed % _replenishmentPeriod.TotalSeconds;
            var remainingSeconds = _replenishmentPeriod.TotalSeconds - elapsedInPeriod;

            if (remainingSeconds >= _replenishmentPeriod.TotalSeconds)
            {
                return 0;
            }

            return (int)Math.Ceiling(remainingSeconds);
        }
    }
}
