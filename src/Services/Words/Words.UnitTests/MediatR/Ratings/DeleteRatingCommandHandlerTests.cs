using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.MediatR.Features.Ratings.Commands.Delete;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Ratings;

[TestFixture]
public class DeleteRatingCommandHandlerTests
{
    private const int UserId = 1;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<ILogger<DeleteRatingCommandHandler>> _loggerMock;
    private DeleteRatingCommandHandler _sut;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(UserId);
        _loggerMock = new Mock<ILogger<DeleteRatingCommandHandler>>();
        _sut = new DeleteRatingCommandHandler(_dbContext, _httpContextAccessorMock.Object, _loggerMock.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldReturnDeletedRatingId()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 1, Rating = 5, UserId = UserId };

        await _dbContext.WordCollectionRatings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();
        
        var command = new DeleteRatingCommand(rating.Id);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().Be(rating.Id);
    }
    
    [Test]
    public async Task Handle_WhenCalled_ShouldDeleteRatingFromDatabase()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 1, Rating = 5, UserId = UserId };

        await _dbContext.WordCollectionRatings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();
        
        var command = new DeleteRatingCommand(rating.Id);
        var result = await _sut.Handle(command, CancellationToken.None);

        _dbContext.WordCollectionRatings.Should().BeEmpty();
    }
    
    [Test]
    public async Task Handle_WhenRatingIsNotExist_ShouldThrowNotFoundException()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 1, Rating = 5, UserId = UserId };

        var command = new DeleteRatingCommand(rating.Id);

        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
    
    [Test]
    public async Task Handle_WhenUserIsNotRatingCreator_ShouldThrowForbiddenException()
    {
        const int wrongUserId = int.MaxValue;
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 1, Rating = 5, UserId = wrongUserId };

        await _dbContext.WordCollectionRatings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();
        
        var command = new DeleteRatingCommand(rating.Id);
        
        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ForbiddenException>();
    }
}