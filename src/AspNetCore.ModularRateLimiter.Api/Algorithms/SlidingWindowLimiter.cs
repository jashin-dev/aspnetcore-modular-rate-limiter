using AspNetCore.ModularRateLimiter.Api.Interfaces;
using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Algorithms
{
    public class SlidingWindowLimiter : IRateLimiterStrategy
    {
        private readonly SlidingWindowRateLimiter _limiter;
        private readonly TimeSpan _window;
        private readonly int _segments;
        private readonly DateTime _startTime = DateTime.UtcNow;

        public SlidingWindowLimiter(int permitLimit, int windowSeconds, int segments, int queueLimit)
        {
            _window = TimeSpan.FromSeconds(windowSeconds);
            _segments = segments;
            _limiter = new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = _window,
                SegmentsPerWindow = segments,
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
            var segmentLength = _window.TotalSeconds / _segments;
            var elapsed = (now - _startTime).TotalSeconds;
            var elapsedInSegment = elapsed % segmentLength;
            var remainingSeconds = segmentLength - elapsedInSegment;

            if (remainingSeconds >= segmentLength)
            {
                return 0;
            }

            return (int)Math.Ceiling(remainingSeconds);
        }
    }
}
