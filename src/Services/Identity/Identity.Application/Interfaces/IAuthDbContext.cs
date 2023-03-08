using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Identity.Application.Interfaces;

public interface IAuthDbContext : IDisposable
{
    public DatabaseFacade Database { get; }
    public DbSet<AppUser> Users { get; set; }
    Task<int> SaveChangesAsync();
}