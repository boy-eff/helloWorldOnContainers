using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Quartz.Jobs;
using Words.BusinessAccess.Services;
using Words.DataAccess;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.Quartz;

[TestFixture]
public class UpdateDailyWordCollectionJobTests
{
    private WordsDbContext _dbContext;
    private Mock<ILogger<UpdateDailyWordCollectionJob>> _loggerMock;
    private IDailyWordCollectionService _dailyWordCollectionService;
    private IJobExecutionContext _jobExecutionContext;
    private UpdateDailyWordCollectionJob _sut;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _loggerMock = new Mock<ILogger<UpdateDailyWordCollectionJob>>();
        _dailyWordCollectionService = new DailyWordCollectionService();
        _jobExecutionContext = JobExecutionContextProvider.GetJobExecutionContext();
        _sut = new UpdateDailyWordCollectionJob(_dbContext, _dailyWordCollectionService, _loggerMock.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Execute_WhenCalled_ShouldPickMostViewedCollection()
    {
        var wordCollectionWithoutViews = WordCollectionBuilder
            .Default()
            .WithId(1)
            .WithName("Default")
            .Build();

        var wordCollectionWithViews = WordCollectionBuilder
            .Default()
            .WithId(2)
            .WithName("Default")
            .WithDailyViews(int.MaxValue)
            .Build();

        await _dbContext.Collections.AddRangeAsync(wordCollectionWithoutViews, wordCollectionWithViews);
        await _dbContext.SaveChangesAsync();

        await _sut.Execute(_jobExecutionContext);

        _dailyWordCollectionService.DailyWordCollection.Should().BeEquivalentTo(wordCollectionWithViews);
    }
    
    [Test]
    public async Task Execute_WhenCalled_ShouldResetDailyViews()
    {
        var wordCollectionWithoutViews = WordCollectionBuilder
            .Default()
            .WithId(1)
            .WithName("Default")
            .Build();

        var wordCollectionWithViews = WordCollectionBuilder
            .Default()
            .WithId(2)
            .WithName("Default")
            .WithDailyViews(int.MaxValue)
            .Build();

        await _dbContext.Collections.AddRangeAsync(wordCollectionWithoutViews, wordCollectionWithViews);
        await _dbContext.SaveChangesAsync();

        var expectedCount = _dbContext.Collections.Count();

        await _sut.Execute(_jobExecutionContext);

        _dbContext.Collections.Count().Should().Be(expectedCount);
        foreach (var collection in _dbContext.Collections)
        {
            collection.DailyViews.Should().Be(0);
        }
    }
}