using AspNetCore.ModularRateLimiter.Api.Configuration;
using System.Text.Json;

namespace AspNetCore.ModularRateLimiter.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRateLimiterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RateLimitingOptions>(configuration.GetSection(RateLimitingOptions.SectionName));
            return services;
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi();
            return services;
        }

        public static IServiceCollection AddHttpJsonConfiguration(this IServiceCollection services)
        {
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            return services;
        }
    }
}
