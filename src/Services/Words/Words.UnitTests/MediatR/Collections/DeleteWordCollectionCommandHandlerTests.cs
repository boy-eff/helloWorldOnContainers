using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Delete;
using Words.DataAccess;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Collections;

[TestFixture]
public class DeleteWordCollectionCommandHandlerTests
{
    private DeleteWordCollectionCommandHandler _sut;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<ILogger<DeleteWordCollectionCommandHandler>> _loggerMock;
    private Mock<IDistributedCache> _cacheMock;

    [SetUp]
    public void Setup()
    {
        const int userId = 1;
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(userId);
        _loggerMock = new Mock<ILogger<DeleteWordCollectionCommandHandler>>();
        _cacheMock = new Mock<IDistributedCache>();

        _sut = new DeleteWordCollectionCommandHandler(_httpContextAccessorMock.Object, _dbContext,
            _loggerMock.Object, _cacheMock.Object);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Test] 
    public async Task Handle_WhenCalled_ShouldReturnDeletedCollectionIdWithoutErrors()
    {
        var wordCollection = WordCollectionBuilder
            .Default()
            .Simple()
            .Build();

        await _dbContext.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteWordCollectionCommand(wordCollection.Id);
        var response = await _sut.Handle(command, CancellationToken.None);

        _dbContext.Collections.Count().Should().Be(0);
        response.Should().NotBeNull();
        response.Value.Should().Be(wordCollection.Id);
        _cacheMock.Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenCollectionIsNotFound_ShouldThrowNotFoundException()
    {
        const int collectionId = 1;
        var command = new DeleteWordCollectionCommand(collectionId);
        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserIsNotACollectionOwner_ShouldThrowForbiddenException()
    {
        const int userId = 0;
        var wordCollection = WordCollectionBuilder
            .Default()
            .Simple()
            .WithUserId(userId)
            .Build();

        await _dbContext.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteWordCollectionCommand(wordCollection.Id);
        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ForbiddenException>();
    }
}