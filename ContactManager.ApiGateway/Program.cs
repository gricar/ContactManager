using Microsoft.AspNetCore.RateLimiting;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

app.UseRateLimiter();

app.MapReverseProxy();

app.UseMetricServer();

app.UseHttpMetrics();

app.MapGet("/", () => "Hello Api Gateway!");

app.Run();
