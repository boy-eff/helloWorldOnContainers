using Achievements.Application.Services;
using Achievements.Domain;
using Achievements.Domain.Contracts;
using Achievements.Domain.Enums;
using Achievements.Domain.Models;
using Achievements.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Achievements.UnitTests;

[TestFixture]
public class UsersAchievementsServiceTests
{
    private UsersAchievementsService _sut;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<ILogger<UsersAchievementsService>> _mockLogger;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<UsersAchievementsService>>();

        _sut = new UsersAchievementsService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    [Test]
    public async Task UpsertUsersAchievementsLevelAsync_WhenAchievementLevelIsNull_ReturnsNull()
    {
        var user = new User();
        const int achievementId = int.MaxValue;

        var result = await _sut.UpsertUsersAchievementsLevelAsync(user, achievementId);

        result.Should().BeNull();
    }

    [Test]
    public async Task UpsertUsersAchievementsLevelAsync_WhenUsersAchievementsIsNull_CallsAddUsersAchievementsAsync()
    {
        var elderAchievementId = (int)AchievementType.Elder;
        var achievementLevel = SeedData.ElderAchievement.Levels.First();
        var user = new User { Id = 1, YearsInAppAmount = achievementLevel.PointsToAchieve };
        
        _mockUnitOfWork.Setup(x => x.UsersAchievementsRepository.GetAsync(elderAchievementId, user.Id)).ReturnsAsync(null as UsersAchievements);

        var result = await _sut.UpsertUsersAchievementsLevelAsync(user, elderAchievementId);

        _mockUnitOfWork.Verify(x => x.UsersAchievementsRepository.AddAsync(It.IsAny<UsersAchievements>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        result.Should().NotBeNull();
    }
    
    [Test]
    public async Task UpsertUsersAchievementsLevelAsync_WhenUsersAchievementsIsNotNull_CallsUpdateUsersAchievementsAsync()
    {
        var elderAchievementId = (int)AchievementType.Elder;
        var achievementLevel = SeedData.ElderAchievement.Levels.First();
        var user = new User { Id = 1, YearsInAppAmount = achievementLevel.PointsToAchieve };
        var usersAchievements = new UsersAchievements() { UserId = user.Id, AchievementId = elderAchievementId, CurrentLevel = 0};
        
        _mockUnitOfWork.Setup(x => x.UsersAchievementsRepository.GetAsync(elderAchievementId, user.Id)).ReturnsAsync(usersAchievements);

        var result = await _sut.UpsertUsersAchievementsLevelAsync(user, elderAchievementId);

        _mockUnitOfWork.Verify(x => x.UsersAchievementsRepository.Update(It.IsAny<UsersAchievements>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        result.Should().NotBeNull();
    }
    
    [Test]
    public async Task UpsertUsersAchievementsLevelAsync_WhenUsersAchievementsIsNotNull_ShouldUpdateUserAchievementLevel()
    {
        var elderAchievementId = (int)AchievementType.Elder;
        var achievementLevel = SeedData.ElderAchievement.Levels.First();
        var user = new User { Id = 1, YearsInAppAmount = achievementLevel.PointsToAchieve };
        var usersAchievements = new UsersAchievements() { UserId = user.Id, AchievementId = elderAchievementId, CurrentLevel = 0};
        
        _mockUnitOfWork.Setup(x => x.UsersAchievementsRepository.GetAsync(elderAchievementId, user.Id)).ReturnsAsync(usersAchievements);

        var result = await _sut.UpsertUsersAchievementsLevelAsync(user, elderAchievementId);

        result.Should().NotBeNull();
        result.CurrentLevel.Should().Be(achievementLevel.Level);
    }
}