using FluentAssertions;
using Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;
using Words.DataAccess;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Collections;

[TestFixture]
public class GetWordCollectionsQueryHandlerTests
{
    private WordsDbContext _dbContext;
    private GetWordCollectionsQueryHandler _sut;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _sut = new GetWordCollectionsQueryHandler(_dbContext);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldReturnWordCollections()
    {
        var defaultBuilder = WordCollectionBuilder.Default().Simple();

        var wordCollection = defaultBuilder.Build();

        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();

        var expectedResult = defaultBuilder.BuildAsResponseDto();

        var query = new GetWordCollectionsQuery();
        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(1);
        result.First().Should().BeEquivalentTo(expectedResult);
    }
}