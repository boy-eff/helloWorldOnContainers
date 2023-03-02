﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Words.DataAccess.Models;

namespace Words.DataAccess;

public class WordsDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public WordsDbContext()
    {
        
    }
    
    public WordsDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Word> Words { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserWord> UserWords { get; set; }
    public virtual DbSet<WordTranslation> WordTranslations { get; set; }
    public virtual DbSet<WordCollection> Collections { get; set; }
    public virtual DbSet<WordCollectionRating> WordCollectionRatings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}