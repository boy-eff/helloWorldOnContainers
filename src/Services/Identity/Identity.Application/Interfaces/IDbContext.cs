namespace Identity.Application.Interfaces;

public interface IDbContext
{
    Task<int> SaveChangesAsync();
}