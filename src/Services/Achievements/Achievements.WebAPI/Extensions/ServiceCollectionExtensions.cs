using Achievements.Application.Contracts;
using Achievements.Application.Services;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Repositories;

namespace Achievements.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUsersAchievementsRepository, UsersAchievementsRepository>();
        services.AddScoped<IAchievementLevelRepository, AchievementLevelRepository>();
        services.AddScoped<IUsersAchievementsService, UsersAchievementsService>();
        services.AddScoped<IUserService, UserService>();
    }
}