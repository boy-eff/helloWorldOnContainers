using System.Security.Claims;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Messages;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Add;
using Words.DataAccess;
using Words.UnitTests.Builders;
using Words.UnitTests.Helpers;

namespace Words.UnitTests.MediatR.Collections;

[TestFixture]
public class AddWordCollectionCommandHandlerTests
{
    private AddWordCollectionCommandHandler _sut;
    private WordsDbContext _dbContext;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private Mock<IPublishEndpoint> _publishEndpointMock;
    private Mock<ILogger<AddWordCollectionCommandHandler>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        const int userId = 1;
        _dbContext = DbContextProvider.GetMemoryContext();
        _httpContextAccessorMock = HttpContextAccessorProvider.MockHttpContextWithUserIdClaim(userId);
        
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _loggerMock = new Mock<ILogger<AddWordCollectionCommandHandler>>();

        _sut = new AddWordCollectionCommandHandler(_dbContext, _httpContextAccessorMock.Object,
            _publishEndpointMock.Object, _loggerMock.Object);
    }
    
    [TearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldReturnWordCollectionResponseDtoWithoutErrors()
    {
        var genericBuilder = WordCollectionBuilder
            .Default()
            .Simple();
        
        var requestDto = genericBuilder.BuildAsRequestDto();

        var expectedResult = genericBuilder.BuildAsResponseDto();

        var command = new AddWordCollectionCommand(requestDto);
        var response = await _sut.Handle(command, CancellationToken.None);
        
        _publishEndpointMock.Verify(x => x.Publish(It.IsAny<WordCollectionCreatedMessage>(), CancellationToken.None), Times.Once);

        _dbContext.Collections.Count().Should().Be(1);
        _dbContext.Collections.First().Should().BeEquivalentTo(expectedResult);
        response.Should().BeEquivalentTo(expectedResult);
    }
}