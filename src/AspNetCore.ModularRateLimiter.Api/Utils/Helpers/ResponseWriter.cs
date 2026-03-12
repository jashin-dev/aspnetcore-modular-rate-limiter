namespace AspNetCore.ModularRateLimiter.Api.Utils.Helpers
{
    public class ResponseWriter
    {
        public static async Task WriteJsonAsync(HttpContext context, object response)
        {
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
