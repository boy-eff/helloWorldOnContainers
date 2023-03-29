using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Quartz;
using Words.BusinessAccess.Options;
using Words.BusinessAccess.Quartz.Jobs;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.Quartz;

[TestFixture]
public class UpdateCachedCollectionsJobTests
{
    private const int CachedCollectionsCount = 3;
    private Mock<IDistributedCache> _cacheMock;
    private WordsDbContext _dbContext;
    private IOptions<WordsRedisCacheOptions> _wordsRedisCacheOptions;
    private Mock<ILogger<UpdateCachedCollectionsJob>> _loggerMock;
    private IJobExecutionContext _jobExecutionContext;
    private UpdateCachedCollectionsJob _sut;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _cacheMock = new Mock<IDistributedCache>();
        _wordsRedisCacheOptions = Options.Create(new WordsRedisCacheOptions()
        {
            CachedCollectionsCount = CachedCollectionsCount, 
            SlidingExpirationTimeInMinutes = 30
        });
        _loggerMock = new Mock<ILogger<UpdateCachedCollectionsJob>>();
        _jobExecutionContext = JobExecutionContextProvider.GetJobExecutionContext();
        _sut = new UpdateCachedCollectionsJob(_cacheMock.Object, _dbContext, _wordsRedisCacheOptions,
            _loggerMock.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [TestCaseSource(nameof(WordCollectionSets))]
    public async Task Execute_WhenCalled_ShouldCacheCollections(WordCollection[] wordCollections)
    {
        await _dbContext.Collections.AddRangeAsync(wordCollections);
        await _dbContext.SaveChangesAsync();
        
        var expectedCacheCollectionsCount = wordCollections.Length < CachedCollectionsCount
            ? wordCollections.Length
            : CachedCollectionsCount;
        
        await _sut.Execute(_jobExecutionContext);
        
        _cacheMock.Verify(x 
            => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), 
            Times.Exactly(expectedCacheCollectionsCount));
    }

    public static IEnumerable<WordCollection>[] WordCollectionSets =
    {
        new WordCollection[]
        {
            new() { Id = 1, Name = "Default" },
        },
        new WordCollection[]
        {
            new() { Id = 1, Name = "Default" },
            new() { Id = 2, Name = "Default" },
            new() { Id = 3, Name = "Default" },
        },
        new WordCollection[]
        {
            new() { Id = 1, Name = "Default" },
            new() { Id = 2, Name = "Default" },
            new() { Id = 3, Name = "Default" },
            new() { Id = 4, Name = "Default" },
            new() { Id = 5, Name = "Default" },
        }
    };
}