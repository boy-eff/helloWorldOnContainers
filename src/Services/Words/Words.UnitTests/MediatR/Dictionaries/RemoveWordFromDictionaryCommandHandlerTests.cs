using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.RemoveWordFromDictionary;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Dictionaries;

[TestFixture]
public class RemoveWordFromDictionaryCommandHandlerTests
{
    private const int UserId = 1;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessor;
    private Mock<ILogger<RemoveWordFromDictionaryCommandHandler>> _logger;
    private RemoveWordFromDictionaryCommandHandler _sut;
    
    [SetUp]
    public void SetUp()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessor = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(UserId);
        _logger = new Mock<ILogger<RemoveWordFromDictionaryCommandHandler>>();
        _sut = new RemoveWordFromDictionaryCommandHandler(_dbContext, _httpContextAccessor.Object, _logger.Object);
    }
    
    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    [Test]
    public async Task Handle_WhenWordExistsInDictionary_ShouldRemoveWordAndReturnWordId()
    {
        const int wordId = 1;
        _dbContext.UserWords.Add(new UserWord { UserId = UserId, WordId = wordId });
        await _dbContext.SaveChangesAsync();

        var command = new RemoveWordFromDictionaryCommand(wordId);

        var result = await _sut.Handle(command, CancellationToken.None);
        
        _dbContext.UserWords.Should().BeEmpty();
    }

    [Test]
    public void Handle_WhenWordDoesNotExistInDictionary_ShouldThrowNotFoundException()
    {
        const int wordId = 2;
        var command = new RemoveWordFromDictionaryCommand(wordId);

        _sut.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
}