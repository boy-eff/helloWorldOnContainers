using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Words.BusinessAccess.Dtos.CollectionRating;
using Shared.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Update;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, CollectionRatingResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UpdateRatingCommandHandler> _logger;

    public UpdateRatingCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<UpdateRatingCommandHandler> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<CollectionRatingResponseDto> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var existingRating = await _dbContext.WordCollectionRatings.FirstOrDefaultAsync(
            x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (existingRating is null)
        {
            _logger.LogInformation("Updating failed: Collection rating with id {RatingId} was not found", request.Id);
            throw new NotFoundException($"Collection with id {request.Id} was not found");
        }

        if (existingRating.UserId != userId)
        {
            _logger.LogInformation("Updating failed: User with id {UserId} has no permission to update rating {RatingId}",
                userId, request.Id);
            throw new ForbiddenException($"Cannot update collection with id {existingRating.Id}");
        }

        var rating = request.RatingDto.Adapt(existingRating);
        _dbContext.WordCollectionRatings.Update(rating);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Word collection rating with id {RatingId} was successfully updated", rating.Id);
        return rating.Adapt<CollectionRatingResponseDto>();
    }
}