using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Features.Collections.Commands;
using Words.BusinessAccess.Features.Collections.Queries;

namespace Words.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class WordCollectionController : ControllerBase
{
    private readonly IMediator _mediator;

    public WordCollectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all collections
    /// </summary>
    /// <response code="200">Returns all collections</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WordCollectionDto>>> GetAsync()
    {
        var query = new GetWordCollectionsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Add new collection
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     POST /api/wordcollection
    ///     {
    ///         "name": "collectionName",
    ///         "englishLevel": 1,
    ///         "words": [
    ///             {
    ///                 "value": "word value in Russian",
    ///                 "translations": [
    ///                     {
    ///                         "translation": "all possible word translations in English"
    ///                     }
    ///                 ]
    ///             }
    ///         ]
    ///     }
    ///     
    /// </remarks>
    /// <response code="200">Returns the newly created collection</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> InsertAsync([FromBody] WordCollectionCreateDto wordCollectionCreateDto)
    {
        var query = new AddWordCollectionCommand(wordCollectionCreateDto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Update collection
    /// </summary>
    /// <response code="200">Returns updated collection id</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> UpdateAsync([FromBody] WordCollectionDto wordCollectionDto)
    {
        var query = new UpdateWordCollectionCommand(wordCollectionDto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Delete collection
    /// </summary>
    /// <response code="200">Returns deleted collection id</response>
    /// <response code="404">If collection is not found</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> DeleteAsync(int id)
    {
        var query = new DeleteWordCollectionCommand(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}