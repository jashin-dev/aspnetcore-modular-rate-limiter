using AspNetCore.ModularRateLimiter.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiterConfiguration(builder.Configuration);
builder.Services.AddApiDocumentation();
builder.Services.AddControllers();
builder.Services.AddHttpJsonConfiguration();

var app = builder.Build();

app.UseApiDocumentation();
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"),
    branch =>
    {
        branch.UseDynamicRateLimiting();
    });
app.MapControllers();

app.Run();