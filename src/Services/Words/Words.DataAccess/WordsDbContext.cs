using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Words.DataAccess.Models;

namespace Words.DataAccess;

public class WordsDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public WordsDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Word> Words { get; set; }
    public DbSet<WordTranslation> WordTranslations { get; set; }
    public DbSet<WordCollection> Collections { get; set; }
    public DbSet<WordCollectionRate> WordCollectionRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WordCollectionRate>()
            .HasOne<User>(x => x.User)
            .WithMany(x => x.CollectionRates)
            .OnDelete(DeleteBehavior.Restrict);
    }
}