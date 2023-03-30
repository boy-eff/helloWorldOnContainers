using System.Collections.ObjectModel;
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

public class UpdateViewsJobTests
{
    private WordsDbContext _dbContext;
    private Mock<ILogger<UpdateViewsJob>> _loggerMock;
    private Mock<IViewsCounterService> _viewsCounterServiceMock;
    private IJobExecutionContext _jobExecutionContext;
    private UpdateViewsJob _sut;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _loggerMock = new Mock<ILogger<UpdateViewsJob>>();
        _viewsCounterServiceMock = new Mock<IViewsCounterService>();
        _jobExecutionContext = JobExecutionContextProvider.GetJobExecutionContext();
        _sut = new UpdateViewsJob(_viewsCounterServiceMock.Object, _dbContext, _loggerMock.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Execute_WhenCalled_ShouldUpdateCollectionViews()
    {
        var collection = WordCollectionBuilder
            .Default()
            .Simple()
            .Build();

        await _dbContext.Collections.AddAsync(collection);
        await _dbContext.SaveChangesAsync();
        
        const int expectedViewsCount = 100;

        var viewsDictionary = new Dictionary<int, int> { { collection.Id, expectedViewsCount } };

        _viewsCounterServiceMock.Setup(x => x.GetViewsAndFlush())
            .Returns(viewsDictionary);

        await _sut.Execute(_jobExecutionContext);

        var resultCollection = await _dbContext.Collections.FindAsync(collection.Id);
        
        resultCollection.DailyViews.Should().Be(expectedViewsCount);
        resultCollection.TotalViews.Should().Be(expectedViewsCount);
    }
}