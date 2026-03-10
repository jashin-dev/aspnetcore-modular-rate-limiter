using AspNetCore.ModularRateLimiter.Api.Algorithms;
using AspNetCore.ModularRateLimiter.Api.Configuration;
using AspNetCore.ModularRateLimiter.Api.Interfaces;
using AspNetCore.ModularRateLimiter.Api.Utils.Constants;

namespace AspNetCore.ModularRateLimiter.Api.Factory
{
    public class RateLimiterFactory
    {
        public static IRateLimiterStrategy Create(RateLimitingOptions options)
        {
            return options.Algorithm switch
            {
                RateLimiterConstants.FixedWindow => new FixedWindowLimiter(
                    options.PermitLimit,
                    options.WindowSeconds,
                    options.QueueLimit),

                RateLimiterConstants.SlidingWindow => new SlidingWindowLimiter(
                    options.PermitLimit,
                    options.WindowSeconds,
                    options.SegmentsPerWindow,
                    options.QueueLimit),

                RateLimiterConstants.TokenBucket => new TokenBucketLimiter(
                    options.TokenLimit,
                    options.TokensPerPeriod,
                    options.ReplenishmentPeriodSeconds,
                    options.QueueLimit),

                RateLimiterConstants.LeakyBucket => new LeakyBucketLimiter(
                    options.PermitLimit,
                    options.QueueLimit),

                _ => throw new InvalidOperationException("Unsupported rate limiter algorithm")
            };
        }
    }
}
