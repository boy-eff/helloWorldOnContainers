using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using Words.BusinessAccess.Dtos.CollectionRating;
using Shared.Exceptions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Queries.GetById;

public class GetRatingByIdQueryHandler : IRequestHandler<GetRatingByIdQuery, CollectionRatingResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<GetRatingByIdQueryHandler> _logger;

    public GetRatingByIdQueryHandler(WordsDbContext dbContext, ILogger<GetRatingByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CollectionRatingResponseDto> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
    {
        var rating = await _dbContext.WordCollectionRatings.FindAsync(request.Id);
        if (rating is null)
        {
            _logger.LogInformation("Failed to retrieve: Id {RatingId} is not valid", request.Id);
            throw new NotFoundException("Invalid id");
        }
        _logger.LogInformation("Rating with id {RatingId} was successfully retrieved", request.Id);
        return rating.Adapt<CollectionRatingResponseDto>();
    }
}