using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.Dtos.CollectionRating;
using Words.BusinessAccess.MediatR.Features.Ratings.Commands.Add;
using Words.BusinessAccess.MediatR.Features.Ratings.Commands.Delete;
using Words.BusinessAccess.MediatR.Features.Ratings.Commands.Update;
using Words.BusinessAccess.MediatR.Features.Ratings.Queries.GetByCollectionId;
using Words.BusinessAccess.MediatR.Features.Ratings.Queries.GetById;

namespace Words.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/rating")]
public class RatingController: ControllerBase
{
    private readonly IMediator _mediator;

    public RatingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get rating by id
    /// </summary>
    /// <response code="200">Returns rating</response>
    /// <response code="404">If rating is not found</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CollectionRatingResponseDto>> GetByIdAsync(int id)
    {
        var query = new GetRatingByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    /// Get ratings by collection id
    /// </summary>
    /// <response code="200">Returns list of ratings</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpGet("collection/{collectionId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<CollectionRatingResponseDto>>> GetByCollectionIdAsync(int collectionId)
    {
        var query = new GetRatingsByCollectionIdQuery(collectionId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Add rating
    /// </summary>
    /// <response code="200">Returns created rating</response>
    /// <response code="400">If user has already rated collection</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CollectionRatingResponseDto>> RateAsync([FromBody] CollectionRatingCreateDto ratingCreateDto)
    {
        var command = new AddRatingCommand(ratingCreateDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Delete rating
    /// </summary>
    /// <response code="200">Returns deleted rating id</response>
    /// <response code="404">If rating is not found</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> DeleteRatingAsync(int id)
    {
        var command = new DeleteRatingCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Update rating
    /// </summary>
    /// <response code="200">Returns updated rating id</response>
    /// <response code="404">If rating is not found</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CollectionRatingResponseDto>> UpdateRatingAsync(int id, [FromBody] CollectionRatingUpdateDto ratingDto)
    {
        var command = new UpdateRatingCommand(id, ratingDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}