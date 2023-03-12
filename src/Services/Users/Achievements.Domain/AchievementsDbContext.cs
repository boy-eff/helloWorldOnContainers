using System.Reflection;
using Achievements.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Achievements.Domain;

public class AchievementsDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AchievementsDbContext()
    {
        
    }
    
    public AchievementsDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AchievementLevel> AchievementLevels { get; set; }
    public DbSet<UsersAchievements> UsersAchievements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}