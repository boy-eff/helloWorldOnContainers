using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.CollectionRating;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Ratings.Queries.GetByCollectionId;

public class GetRatingsByCollectionIdQueryHandler : IRequestHandler<GetRatingsByCollectionIdQuery, IEnumerable<CollectionRatingResponseDto>>
{
    private readonly WordsDbContext _dbContext;

    public GetRatingsByCollectionIdQueryHandler(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CollectionRatingResponseDto>> Handle(GetRatingsByCollectionIdQuery request, CancellationToken cancellationToken)
    {
        var ratings = await _dbContext.WordCollectionRatings
            .Where(x => x.CollectionId == request.CollectionId)
            .ToListAsync(cancellationToken: cancellationToken);

        return ratings.Adapt<IEnumerable<CollectionRatingResponseDto>>();

    }
}