using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Shared.Messages;
using Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.AddWordToDictionary;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Dictionaries;

[TestFixture]
public class AddWordToDictionaryCommandHandlerTests
{
    private const int UserId = 1;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessor;
    private Mock<IPublishEndpoint> _publishEndpoint;
    private Mock<ILogger<AddWordToDictionaryCommandHandler>> _logger;
    private AddWordToDictionaryCommandHandler _sut;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessor = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(UserId);
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _logger = new Mock<ILogger<AddWordToDictionaryCommandHandler>>();

        _sut = new AddWordToDictionaryCommandHandler(_dbContext, _httpContextAccessor.Object, _publishEndpoint.Object, _logger.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldReturnWordId()
    {
        var word = new Word { Id = 1, Value = "Test" };
        await _dbContext.Words.AddAsync(word);
        await _dbContext.SaveChangesAsync();

        var request = new AddWordToDictionaryCommand(word.Id);

        var result = await _sut.Handle(request, CancellationToken.None);
        
        result.Should().Be(word.Id);
        _publishEndpoint.Verify(x 
            => x.Publish(It.IsAny<WordAddedToDictionaryMessage>(), CancellationToken.None), Times.Once);
    }
    
    [Test]
    public async Task Handle_WhenCalled_ShouldAddWordToDatabase()
    {
        var word = new Word { Id = 1, Value = "Test" };
        await _dbContext.Words.AddAsync(word);
        await _dbContext.SaveChangesAsync();

        var request = new AddWordToDictionaryCommand(word.Id);

        var result = await _sut.Handle(request, CancellationToken.None);

        var userWords = await _dbContext.UserWords.ToListAsync();
        userWords.Should().NotBeNullOrEmpty();
        userWords.Count().Should().Be(1);
        userWords.First().WordId.Should().Be(word.Id);
        userWords.First().UserId.Should().Be(UserId);
    }
    
    [Test]
    public async Task Handle_WhenCalled_ShouldPublishEvent()
    {
        var word = new Word { Id = 1, Value = "Test" };
        await _dbContext.Words.AddAsync(word);
        await _dbContext.SaveChangesAsync();

        var request = new AddWordToDictionaryCommand(word.Id);

        await _sut.Handle(request, CancellationToken.None);
        
        _publishEndpoint.Verify(x 
            => x.Publish(It.IsAny<WordAddedToDictionaryMessage>(), CancellationToken.None), Times.Once);
    }
    
    [Test]
    public void Handle_WhenWordDoesNotExist_ShouldThrowNotFoundException()
    {
        var request = new AddWordToDictionaryCommand(1);

        _sut.Invoking(x => x.Handle(request, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
        _dbContext.UserWords.Should().BeEmpty();
        
        _publishEndpoint.Verify(x 
                => x.Publish(It.IsAny<WordAddedToDictionaryMessage>(), CancellationToken.None), Times.Never);
    }
}