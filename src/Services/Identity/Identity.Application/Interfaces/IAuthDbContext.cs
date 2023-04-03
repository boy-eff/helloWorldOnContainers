namespace Identity.Application.Interfaces;

public interface IAuthDbContext
{
    public Task<int> SaveChangesAsync();
}