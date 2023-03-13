using Achievements.Application.Extensions;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Achievements.Domain.Repositories;
using Achievements.WebAPI.Extensions;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AchievementsDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUsersAchievementsRepository, UsersAchievementsRepository>();
builder.Services.AddScoped<IAchievementLevelRepository, AchievementLevelRepository>();

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

await app.ApplyMigrations();


app.Run();