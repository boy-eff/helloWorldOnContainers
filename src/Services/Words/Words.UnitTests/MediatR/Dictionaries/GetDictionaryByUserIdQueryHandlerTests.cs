using FluentAssertions;
using Mapster;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.MediatR.Features.Dictionaries.Queries.GetDictionary;
using Words.DataAccess;
using Words.DataAccess.Models;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Dictionaries;

[TestFixture]
public class GetDictionaryByUserIdQueryHandlerTests
{
    private readonly List<User> _fakeUsers = new()
    {
        new User { Id = 1, UserName = "testuser" },
        new User { Id = 2, UserName = "testuser2" }
    };
    private readonly List<Word> _fakeWords = new()
    {
        new Word { Id = 1, Value = "hello" },
        new Word { Id = 2, Value = "world" }
    };

    private List<UserWord> _fakeUserWords = new()
    {
        new UserWord { UserId = 1, WordId = 1 },
        new UserWord { UserId = 2, WordId = 2 }
    };
    
    private WordsDbContext _dbContext;
    private Mock<ILogger<GetDictionaryByUserIdQueryHandler>> _loggerMock;
    private GetDictionaryByUserIdQueryHandler _sut;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        SeedDatabase();
        _loggerMock = new Mock<ILogger<GetDictionaryByUserIdQueryHandler>>();
        _sut = new GetDictionaryByUserIdQueryHandler(_dbContext, _loggerMock.Object);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    public async Task Handle_WhenUserExists_ReturnsWords(int userId)
    {
        var userWordIds = _fakeUserWords.Where(x => x.UserId == userId).Select(x => x.WordId);

        var expectedResult = _fakeWords.Where(x => userWordIds.Contains(x.Id)).Adapt<IEnumerable<WordDto>>();

        var query = new GetDictionaryByUserIdQuery(userId); 

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void Handle_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        var query = new GetDictionaryByUserIdQuery(Int32.MaxValue);

        _sut.Invoking(x => x.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }

    private void SeedDatabase()
    {
        _dbContext.Users.AddRange(_fakeUsers);
        _dbContext.Words.AddRange(_fakeWords);
        _dbContext.UserWords.AddRange(_fakeUserWords);
        _dbContext.SaveChanges();
    }
}