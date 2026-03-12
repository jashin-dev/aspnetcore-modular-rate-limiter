using AspNetCore.ModularRateLimiter.Api.Interfaces;
using System.Threading.RateLimiting;

namespace AspNetCore.ModularRateLimiter.Api.Algorithms
{
    public class FixedWindowLimiter : IRateLimiterStrategy
    {
        private readonly FixedWindowRateLimiter _limiter;
        private readonly TimeSpan _window;
        private readonly DateTime _startTime = DateTime.UtcNow;

        public FixedWindowLimiter(int permitLimit, int windowSeconds, int queueLimit)
        {
            _window = TimeSpan.FromSeconds(windowSeconds);
            _limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = _window,
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
            var elapsedInCurrentWindow = (now - _startTime).TotalSeconds % _window.TotalSeconds;
            var remainingSeconds = _window.TotalSeconds - elapsedInCurrentWindow;

            if (remainingSeconds >= _window.TotalSeconds)
            {
                return 0;
            }

            return (int)Math.Ceiling(remainingSeconds);
        }
    }
}
