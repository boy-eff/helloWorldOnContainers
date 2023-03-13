using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos.CollectionRating;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Update;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, CollectionRatingResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateRatingCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CollectionRatingResponseDto> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var existingRating = await _dbContext.WordCollectionRatings.FirstOrDefaultAsync(
            x => x.Id == request.Id && x.UserId == userId, cancellationToken: cancellationToken);

        if (existingRating == null)
        {
            throw new NotFoundException("Rating is not found");
        }

        var rating = request.RatingDto.Adapt(existingRating);
        _dbContext.WordCollectionRatings.Update(rating);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return rating.Adapt<CollectionRatingResponseDto>();
    }
}