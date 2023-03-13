using Achievements.Domain.Contracts;
using Achievements.Domain.Repositories;

namespace Achievements.Domain;

public class UnitOfWork : IUnitOfWork
{
    private readonly AchievementsDbContext _dbContext;
    private IUserRepository _userRepository;
    private IUsersAchievementsRepository _usersAchievementsRepository;
    private IAchievementLevelRepository _achievementLevelRepository;

    public UnitOfWork(AchievementsDbContext dbContext, 
        IUserRepository userRepository, 
        IUsersAchievementsRepository usersAchievementsRepository,
        IAchievementLevelRepository achievementLevelRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _usersAchievementsRepository = usersAchievementsRepository;
        _achievementLevelRepository = achievementLevelRepository;
    }

    public IUserRepository UserRepository
    {
        get { return _userRepository = _userRepository ?? new UserRepository(_dbContext); }
    }
    
    public IUsersAchievementsRepository UsersAchievementsRepository
    {
        get { return _usersAchievementsRepository = _usersAchievementsRepository ?? new UsersAchievementsRepository(_dbContext); }
    }

    public IAchievementLevelRepository AchievementLevelRepository
    {
        get { return _achievementLevelRepository = _achievementLevelRepository ?? new AchievementLevelRepository(_dbContext); }
    }

    public void Commit()
    {
        _dbContext.Database.CommitTransaction();
    }

    public void Rollback()
    {
        _dbContext.Database.RollbackTransaction();
    }

    public async Task CommitAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}