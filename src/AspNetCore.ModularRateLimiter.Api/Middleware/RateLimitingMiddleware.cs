using AspNetCore.ModularRateLimiter.Api.Configuration;
using AspNetCore.ModularRateLimiter.Api.Factory;
using AspNetCore.ModularRateLimiter.Api.Interfaces;
using AspNetCore.ModularRateLimiter.Api.Models;
using AspNetCore.ModularRateLimiter.Api.Utils.Helpers;
using Microsoft.Extensions.Options;
using System.Net;

namespace AspNetCore.ModularRateLimiter.Api.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private IRateLimiterStrategy _limiter;
        private readonly IOptionsMonitor<RateLimitingOptions> _configuration;

        public RateLimitingMiddleware(
            RequestDelegate next,
            ILogger<RateLimitingMiddleware> logger,
            IOptionsMonitor<RateLimitingOptions> configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
            _limiter = RateLimiterFactory.Create(configuration.CurrentValue);

            _logger.LogInformation("Rate limiter initialized with {Algorithm}", configuration.CurrentValue.Algorithm);
            _configuration.OnChange(configuration =>
            {
                _logger.LogInformation("Rate limiter configuration changed. Configured algorithm: {Algorithm}", configuration.Algorithm);
                _limiter = RateLimiterFactory.Create(configuration);
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            using var lease = await _limiter.AcquireAsync();

            if (!lease.IsAcquired)
            {
                _logger.LogWarning("Rate limit exceeded for IP {IP}", ip);
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;

                int retryAfterSeconds = _limiter.GetRetryAfterSeconds();
                context.Response.Headers.RetryAfter = retryAfterSeconds.ToString();

                var response = new ApiResponse
                {
                    Success = false,
                    Message = "Rate limit exceeded. Please try again later.",
                    Path = context.Request.Path
                };

                await ResponseWriter.WriteJsonAsync(context, response);
                return;
            }

            await _next(context);
        }
    }
}
