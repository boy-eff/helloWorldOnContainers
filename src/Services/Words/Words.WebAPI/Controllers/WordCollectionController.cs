﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Add;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Delete;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;
using Words.BusinessAccess.MediatR.Features.Collections.Commands.UpdateModerationStatus;
using Words.BusinessAccess.MediatR.Features.Collections.Queries.Get;
using Words.BusinessAccess.MediatR.Features.Collections.Queries.GetById;

namespace Words.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    public async Task<ActionResult<List<WordCollectionResponseDto>>> GetAsync()
    {
        var query = new GetWordCollectionsQuery();
        var result = await _mediator.Send(query, CancellationToken.None);
        return Ok(result);
    }
    
    /// <summary>
    /// Get collection by id
    /// </summary>
    /// <response code="200">Returns a collection</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="404">If collection is not found</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WordCollectionResponseDto>> GetByIdAsync(int id)
    {
        var query = new GetWordCollectionByIdQuery(id);
        var result = await _mediator.Send(query, CancellationToken.None);
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
    public async Task<ActionResult<int>> InsertAsync([FromBody] WordCollectionRequestDto wordCollectionCreateDto)
    {
        var command = new AddWordCollectionCommand(wordCollectionCreateDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Update collection
    /// </summary>
    /// <response code="200">Returns updated collection id</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="403">If user has no permission to update collection</response>
    /// <response code="404">If collection is not found</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> UpdateAsync(int id,[FromBody] WordCollectionRequestDto wordCollectionDto)
    {
        var command = new UpdateWordCollectionCommand(id, wordCollectionDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Delete collection
    /// </summary>
    /// <response code="200">Returns deleted collection id</response>
    /// <response code="404">If collection is not found</response>
    /// <response code="403">If user has no permission to delete collection</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> DeleteAsync(int id)
    {
        var command = new DeleteWordCollectionCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    /// <summary>
    /// Update collection moderation status
    /// </summary>
    /// <response code="200">Returns updated collection id</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="404">If collection is not found</response>
    [HttpPut("moderate")]
    [Authorize(Policies.ModeratorOnly)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> UpdateModerationStatusAsync([FromBody] WordCollectionModerationDto wordCollectionDto)
    {
        var command = new UpdateModerationStatusCommand(wordCollectionDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}