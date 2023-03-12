using Achievements.Application.Extensions;
using Achievements.Domain;
using Achievements.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheUrl"];
    options.InstanceName = "local";
});
var app = builder.Build();

var cache = app.Services.GetService<IDistributedCache>();
await cache.SetAsync<IEnumerable<Achievement>>(nameof(Achievement), SeedData.Achievements);

app.MapGet("/", () =>
{
    IEnumerable<Achievement> achievements;
    cache.TryGetValue("achievements", out achievements);
    return achievements;
});


app.Run();