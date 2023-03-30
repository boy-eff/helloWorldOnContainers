using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos.CollectionRating;
using Words.BusinessAccess.MediatR.Features.Ratings.Commands.Add;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Ratings;

[TestFixture]
public class AddRatingCommandHandlerTests
{
    private const int UserId = 1;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<ILogger<AddRatingCommandHandler>> _loggerMock;
    private AddRatingCommandHandler _sut;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(UserId);
        _loggerMock = new Mock<ILogger<AddRatingCommandHandler>>();
        _sut = new AddRatingCommandHandler(_dbContext, _httpContextAccessorMock.Object, _loggerMock.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldAddReturnRatingResponseDto()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 10, UserId = UserId };
        var wordCollection = new WordCollection() { Id = rating.CollectionId, Name = "Test" };
        
        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        
        var ratingCreateDto = new CollectionRatingRequestDto() { Rating = rating.Rating };
        var command = new AddRatingCommand(rating.CollectionId, ratingCreateDto);

        var expectedResult = rating.Adapt<CollectionRatingResponseDto>();

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Test]
    public async Task Handle_WhenCalled_ShouldSaveRatingToDatabase()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 10, UserId = UserId };
        var wordCollection = new WordCollection() { Id = rating.CollectionId, Name = "Test" };
        
        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        
        var ratingCreateDto = new CollectionRatingRequestDto() { Rating = rating.Rating };
        var command = new AddRatingCommand(rating.CollectionId, ratingCreateDto);

        var expectedResult = rating.Adapt<CollectionRatingResponseDto>();

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Test]
    public async Task Handle_WhenWordCollectionDoesNotExist_ShouldThrowNotFoundException()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 10, UserId = UserId };

        var ratingCreateDto = new CollectionRatingRequestDto() { Rating = rating.Rating };
        var command = new AddRatingCommand(rating.CollectionId, ratingCreateDto);

        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
        
        _dbContext.WordCollectionRatings.Should().BeEmpty();
    }

    [Test]
    public async Task Handle_WhenRatingIsAlreadyExist_ShouldThrowWrongActionException()
    {
        var rating = new WordCollectionRating() { Id = 1, CollectionId = 10, UserId = UserId };
        var wordCollection = new WordCollection() { Id = rating.CollectionId, Name = "Test" };
        
        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.WordCollectionRatings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();

        var ratingCreateDto = new CollectionRatingRequestDto() { Rating = rating.Rating };
        var command = new AddRatingCommand(rating.CollectionId, ratingCreateDto);

        await _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<WrongActionException>();
    }
}