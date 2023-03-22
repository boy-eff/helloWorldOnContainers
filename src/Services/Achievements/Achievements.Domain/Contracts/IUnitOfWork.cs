﻿namespace Achievements.Domain.Contracts;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IUsersAchievementsRepository UsersAchievementsRepository { get; }
    IAchievementLevelRepository AchievementLevelRepository { get; }
    void Commit();
    void Rollback();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveChangesAsync();
}