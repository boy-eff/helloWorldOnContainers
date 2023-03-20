using Achievements.Application.Contracts;
using Achievements.Application.Extensions;
using Achievements.Application.Services;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Models;
using Achievements.Domain.Repositories;
using Achievements.WebAPI.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddDbContext<AchievementsDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUsersAchievementsRepository, UsersAchievementsRepository>();
builder.Services.AddScoped<IAchievementLevelRepository, AchievementLevelRepository>();
builder.Services.AddScoped<IUsersAchievementsService, UsersAchievementsService>();
builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);

builder.Services.ConfigureRedis(config);
builder.Services.ConfigureMassTransit(config);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

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