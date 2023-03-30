using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos.CollectionRating;
using Words.BusinessAccess.MediatR.Features.Ratings.Commands.Update;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Ratings;

[TestFixture]
public class UpdateRatingCommandHandlerTests
{
    private const int UserId = 1;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<ILogger<UpdateRatingCommandHandler>> _loggerMock;
    private UpdateRatingCommandHandler _sut;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(UserId);
        _loggerMock = new Mock<ILogger<UpdateRatingCommandHandler>>();
        _sut = new UpdateRatingCommandHandler(_dbContext, _httpContextAccessorMock.Object, _loggerMock.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldReturnUpdatedRating()
    {
        var existingRating = new WordCollectionRating() { Id = 1, CollectionId = 1, UserId = UserId, Rating = 1 };
        await _dbContext.WordCollectionRatings.AddAsync(existingRating);
        await _dbContext.SaveChangesAsync();

        var requestDto = new CollectionRatingRequestDto() { Rating = existingRating.Rating + 1 };
        var command = new UpdateRatingCommand(existingRating.Id, requestDto);

        var expectedResult = existingRating.Adapt<CollectionRatingResponseDto>();
        expectedResult.Rating = requestDto.Rating;

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResult);
    }
    
        [Test]
    public async Task Handle_WhenCalled_ShouldUpdateRatingInDatabase()
    {
        var existingRating = new WordCollectionRating() { Id = 1, CollectionId = 1, UserId = UserId, Rating = 1 };
        await _dbContext.WordCollectionRatings.AddAsync(existingRating);
        await _dbContext.SaveChangesAsync();

        var requestDto = new CollectionRatingRequestDto() { Rating = existingRating.Rating + 1 };
        var command = new UpdateRatingCommand(existingRating.Id, requestDto);

        var expectedResult = existingRating.Adapt<WordCollectionRating>();
        expectedResult.Rating = requestDto.Rating;

        await _sut.Handle(command, CancellationToken.None);

        _dbContext.WordCollectionRatings.Count().Should().Be(1);
        var dbRating = await _dbContext.WordCollectionRatings.FirstOrDefaultAsync();
        dbRating.Should().BeEquivalentTo(expectedResult);
    }
    
    [Test]
    public async Task Handle_WhenRatingIsNotExist_ShouldThrowNotFoundException()
    {
        const int ratingId = 1;
        var requestDto = new CollectionRatingRequestDto() { Rating = 1 };
        var command = new UpdateRatingCommand(ratingId, requestDto);

        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
    
    [Test]
    public async Task Handle_WhenUserIsNotRatingCreator_ShouldThrowForbiddenException()
    {
        const int wrongUserId = int.MaxValue;
        var existingRating = new WordCollectionRating() { Id = 1, CollectionId = 1, UserId = wrongUserId, Rating = 1 };
        await _dbContext.WordCollectionRatings.AddAsync(existingRating);
        await _dbContext.SaveChangesAsync();
        
        var requestDto = new CollectionRatingRequestDto() { Rating = 1 };
        var command = new UpdateRatingCommand(existingRating.Id, requestDto);

        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ForbiddenException>();
    }
}