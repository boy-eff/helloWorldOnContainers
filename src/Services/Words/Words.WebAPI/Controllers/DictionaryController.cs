using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.AddWordToDictionary;
using Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.RemoveWordFromDictionary;
using Words.BusinessAccess.MediatR.Features.Dictionaries.Queries.GetDictionary;

namespace Words.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DictionaryController: ControllerBase
{
    private readonly IMediator _mediator;

    public DictionaryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Add word to dictionary
    /// </summary>
    /// <response code="200">Returns added word id</response>
    /// <response code="404">If word is not found</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> AddWordToDictionaryAsync([FromBody]int wordId)
    {
        var command = new AddWordToDictionaryCommand(wordId);
        var result = await _mediator.Send(command, CancellationToken.None);
        return Ok(result);
    }
    
    /// <summary>
    /// Get dictionary by user id
    /// </summary>
    /// <response code="200">Returns a dictionary</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="404">If dictionary is not found</response>
    [HttpGet("{dictionaryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<WordDto>>> GetDictionaryByUserIdAsync([FromRoute]int dictionaryId)
    {
        var query = new GetDictionaryByUserIdQuery(dictionaryId);
        var result = await _mediator.Send(query, CancellationToken.None);
        return Ok(result);
    }
    
    /// <summary>
    /// Remove word from dictionary
    /// </summary>
    /// <response code="200">Returns deleted word id</response>
    /// <response code="404">If dictionary is not found</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpDelete("{wordId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> RemoveWordFromDictionaryAsync([FromRoute]int wordId)
    {
        var command = new RemoveWordFromDictionaryCommand(wordId);
        var result = await _mediator.Send(command, CancellationToken.None);
        return Ok(result);
    }
}