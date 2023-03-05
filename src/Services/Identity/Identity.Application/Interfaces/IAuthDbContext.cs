using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Identity.Application.Interfaces;

public interface IAuthDbContext : IDisposable
{
    public DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync();
}