using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.MediatR.Features.Collections.Queries.GetById;
using Words.DataAccess;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Collections;

[TestFixture]
public class GetWordCollectionByIdQueryHandlerTests
{
    private WordsDbContext _dbContext;
    private Mock<IViewsCounterService> _viewsCounterServiceMock;
    private Mock<IDistributedCache> _cacheMock;
    private Mock<ILogger<GetWordCollectionByIdQueryHandler>> _loggerMock;
    private GetWordCollectionByIdQueryHandler _sut;

    [SetUp]
    public void SetUp()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _viewsCounterServiceMock = new Mock<IViewsCounterService>();
        _cacheMock = new Mock<IDistributedCache>();
        _loggerMock = new Mock<ILogger<GetWordCollectionByIdQueryHandler>>();
        _sut = new GetWordCollectionByIdQueryHandler(_dbContext, _viewsCounterServiceMock.Object, _cacheMock.Object, _loggerMock.Object);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }
    
    [Test]
    public async Task Handle_WhenCalled_ShouldReturnWordCollection()
    {
        var collectionId = 1;
        var defaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(collectionId);
        
        var request = new GetWordCollectionByIdQuery(collectionId);
        var wordCollection = defaultBuilder.Build();

        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();

        _cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns((byte[]?)null);

        var expectedResult = defaultBuilder.BuildAsResponseDto();
        
        var response = await _sut.Handle(request, CancellationToken.None);

        _viewsCounterServiceMock.Verify(x => x.IncrementViewsInCollection(collectionId, 1), Times.Once);
        _cacheMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
        response.Should().BeEquivalentTo(expectedResult);
    }
    
    [Test]
    public void Handle_WhenCollectionDoesNotExist_ShouldThrowNotFoundException()
    {
        var collectionId = 1;
        var request = new GetWordCollectionByIdQuery(collectionId);
        
        _cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns((byte[]?)null);

        _sut.Invoking(x => x.Handle(request, CancellationToken.None)).Should().ThrowAsync<NotFoundException>();
        
        _viewsCounterServiceMock.Verify(x => x.IncrementViewsInCollection(collectionId, It.IsAny<int>()), Times.Never);
    }
    
    [Test]
    public async Task Handle_WhenCollectionIsInCache_ShouldReturnCachedResponse()
    {
        // Arrange
        const int collectionId = 1;
        var request = new GetWordCollectionByIdQuery(collectionId);
        
        var defaultBuilder = WordCollectionBuilder
            .Default()
            .WithId(collectionId)
            .Simple();

        var wordCollection = defaultBuilder.Build();

        var expectedResult = defaultBuilder.BuildAsResponseDto();
        
        _cacheMock.Setup(x => x.Get(It.IsAny<string>())).Returns(SerializationHelper.SerializeObject(wordCollection));

        // Act
        var response = await _sut.Handle(request, CancellationToken.None);

        // Assert
        _viewsCounterServiceMock.Verify(x => x.IncrementViewsInCollection(collectionId, It.IsAny<int>()), Times.Once);
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedResult);
    }
}