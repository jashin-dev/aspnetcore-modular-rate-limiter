using System.Text.Json;

namespace AspNetCore.ModularRateLimiter.Api.Utils.Helpers
{
    public class ResponseWriter
    {
        public static async Task WriteJsonAsync(HttpContext context, object response)
        {
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
