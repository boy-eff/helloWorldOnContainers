using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Exceptions;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;
using Words.BusinessAccess.Options;
using Words.DataAccess;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Collections;

[TestFixture]
public class UpdateWordCollectionCommandHandlerTests
{
    private const int UserId = 1;
    private UpdateWordCollectionCommandHandler _sut;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<ILogger<UpdateWordCollectionCommandHandler>> _loggerMock;
    private Mock<IDistributedCache> _cacheMock;
    
    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(UserId);
        _loggerMock = new Mock<ILogger<UpdateWordCollectionCommandHandler>>();
        _cacheMock = new Mock<IDistributedCache>();

        var options = Options.Create(new WordsRedisCacheOptions() { CachedCollectionsCount = 10, SlidingExpirationTimeInMinutes = 30 });

        _sut = new UpdateWordCollectionCommandHandler(_httpContextAccessorMock.Object, _dbContext, _cacheMock.Object,
            options, _loggerMock.Object);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCollectionIsNotCached_ShouldReturnWordCollectionResponseDto()
    {
        const int wordCollectionId = 1;
        var wordCollectionDefaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .WithUserId(UserId);

        var wordCollection = wordCollectionDefaultBuilder
            .Build();

        var requestDto = wordCollectionDefaultBuilder
            .BuildAsRequestDto();

        var expectedResponse = wordCollectionDefaultBuilder
            .BuildAsResponseDto();
        
        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        
        _cacheMock.Setup(x 
            => x.Get(It.IsAny<string>())).Returns((byte[]?)null);

        var command = new UpdateWordCollectionCommand(wordCollectionId, requestDto);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Id.Should().Be(expectedResponse.Id);
        result.Name.Should().Be(expectedResponse.Name);
        result.EnglishLevel.Should().Be(expectedResponse.EnglishLevel);
    }
    
    [Test]
    public async Task Handle_WhenCollectionIsNotCached_ShouldNotCacheUpdatedCollections()
    {
        const int wordCollectionId = 1;
        var wordCollectionDefaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .WithUserId(UserId);

        var wordCollection = wordCollectionDefaultBuilder
            .Build();

        var requestDto = wordCollectionDefaultBuilder
            .BuildAsRequestDto();

        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        
        _cacheMock.Setup(x 
            => x.Get(It.IsAny<string>())).Returns((byte[]?)null);

        var command = new UpdateWordCollectionCommand(wordCollectionId, requestDto);
        await _sut.Handle(command, CancellationToken.None);

        _cacheMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
        _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), 
                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Exactly(0));
    }
    
    [Test]
    public async Task Handle_WhenCollectionIsCached_ShouldCacheUpdatedCollections()
    {
        const int wordCollectionId = 1;
        var wordCollectionDefaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .WithUserId(UserId);

        var wordCollection = wordCollectionDefaultBuilder
            .Build();

        var requestDto = wordCollectionDefaultBuilder
            .BuildAsRequestDto();

        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(wordCollection).State = EntityState.Detached;

        _cacheMock.Setup(x 
            => x.Get(It.IsAny<string>())).Returns(SerializationHelper.SerializeObject(wordCollection));

        var command = new UpdateWordCollectionCommand(wordCollectionId, requestDto);
        await _sut.Handle(command, CancellationToken.None);

        _cacheMock.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
        _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), 
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task Handle_WhenCollectionIsCached_ShouldReturnWordCollectionResponseDto()
    {
        const int wordCollectionId = 1;
        var wordCollectionDefaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .WithUserId(UserId);

        var wordCollection = wordCollectionDefaultBuilder
            .Build();
        
        await _dbContext.Collections.AddAsync(wordCollection);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(wordCollection).State = EntityState.Detached;

        _cacheMock.Setup(x => x.Get(It.IsAny<string>()))
            .Returns(SerializationHelper.SerializeObject(wordCollection));

        var requestDto = wordCollectionDefaultBuilder
            .BuildAsRequestDto();

        var expectedResponse = wordCollectionDefaultBuilder
            .BuildAsResponseDto();

        var command = new UpdateWordCollectionCommand(wordCollectionId, requestDto);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.Id.Should().Be(expectedResponse.Id);
        result.Name.Should().Be(expectedResponse.Name);
        result.EnglishLevel.Should().Be(expectedResponse.EnglishLevel);
    }

    [Test]
    public async Task Handle_WhenCollectionIsNotFound_ShouldThrowNotFoundException()
    {
        const int wordCollectionId = 1;
        var wordCollectionDefaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .WithUserId(UserId);
        
        _cacheMock.Setup(x => x.Get(It.IsAny<string>()))
            .Returns((byte[]?)null);

        var requestDto = wordCollectionDefaultBuilder
            .BuildAsRequestDto();

        var command = new UpdateWordCollectionCommand(wordCollectionId, requestDto);
        await _sut.Invoking(async x => await x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<NotFoundException>();
    }
    
    [Test]
    public async Task Handle_WhenUserIsNotCollectionCreator_ShouldThrowForbiddenException()
    {
        const int wordCollectionId = 1;
        const int wrongUserId = UserId + 1;
        var wordCollectionDefaultBuilder = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .WithUserId(UserId);

        var wordCollection = wordCollectionDefaultBuilder.Build();
        wordCollection.UserId = wrongUserId;
        
        _cacheMock.Setup(x => x.Get(It.IsAny<string>()))
            .Returns(SerializationHelper.SerializeObject(wordCollection));

        var requestDto = wordCollectionDefaultBuilder
            .BuildAsRequestDto();

        var command = new UpdateWordCollectionCommand(wordCollectionId, requestDto);
        await _sut.Invoking(async x => await x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<ForbiddenException>();
    }
}