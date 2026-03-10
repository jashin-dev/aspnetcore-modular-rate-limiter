using AspNetCore.ModularRateLimiter.Api.Middleware;
using Scalar.AspNetCore;

namespace AspNetCore.ModularRateLimiter.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDynamicRateLimiting(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimitingMiddleware>();
        }

        public static WebApplication UseApiDocumentation(this WebApplication app)
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.Title = "ASP.NET Core Modular Rate Limiter API";
            });

            return app;
        }
    }
}
