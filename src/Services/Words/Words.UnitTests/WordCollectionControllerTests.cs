using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Add;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Delete;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;
using Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;
using Words.UnitTests.Builders;
using Words.WebAPI.Controllers;

namespace Words.UnitTests;

public class WordCollectionControllerTests
{
    private WordCollectionController _sut;
    private Mock<IMediator> _mediatorMock;
    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _sut = new WordCollectionController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetAsync_WhenCalled_ShouldReturn200Ok()
    {
        var wordCollectionDto = WordCollectionBuilder
            .Default()
            .Simple()
            .BuildAsResponseDto();

        var wordCollections = new List<WordCollectionResponseDto>()
        {
            wordCollectionDto
        };
        
        _mediatorMock.Setup(x => x.Send(
                It.IsAny<GetWordCollectionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(wordCollections.AsEnumerable);

        var result = await _sut.GetAsync();

        result.Result.Should().BeAssignableTo<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(wordCollections);
    }
    
    [Test]
    public async Task InsertAsync_WhenCalled_ShouldReturn200Ok()
    {
        var wordCollectionBuilder = WordCollectionBuilder
            .Default()
            .Simple();

        var wordCollectionDto = wordCollectionBuilder.BuildAsResponseDto();
        var wordCollectionCreateDto = wordCollectionBuilder.BuildAsRequestDto();
        

        _mediatorMock.Setup(x => x.Send(
                It.IsAny<AddWordCollectionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(wordCollectionDto);

        var result = await _sut.InsertAsync(wordCollectionCreateDto);

        result.Result.Should().BeAssignableTo<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(wordCollectionDto);
    }
    
    [Test]
    public async Task UpdateAsync_WhenCalled_ShouldReturn200Ok()
    {
        const int wordCollectionId = 1;
        
        var wordCollectionRequestDto = WordCollectionBuilder
            .Default()
            .Simple()
            .BuildAsRequestDto();
        
        var wordCollectionResponseDto = WordCollectionBuilder
            .Default()
            .Simple()
            .WithId(wordCollectionId)
            .BuildAsResponseDto();
        
        _mediatorMock.Setup(x => x.Send(
                It.IsAny<UpdateWordCollectionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(wordCollectionResponseDto);

        var result = await _sut.UpdateAsync(wordCollectionId, wordCollectionRequestDto);

        result.Result.Should().BeAssignableTo<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(wordCollectionResponseDto);
    }
    
    [Test]
    public async Task DeleteAsync_WhenCalled_ShouldReturn200Ok()
    {
        const int wordCollectionId = 1;

        _mediatorMock.Setup(x => x.Send(
                It.IsAny<DeleteWordCollectionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(wordCollectionId);

        var result = await _sut.DeleteAsync(wordCollectionId);

        result.Result.Should().BeAssignableTo<OkObjectResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(wordCollectionId);
    }
}