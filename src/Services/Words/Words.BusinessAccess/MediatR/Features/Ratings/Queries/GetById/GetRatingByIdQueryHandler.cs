using Mapster;
using MediatR;
using Words.BusinessAccess.Dtos.CollectionRating;
using Words.BusinessAccess.Exceptions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Queries.GetById;

public class GetRatingByIdQueryHandler : IRequestHandler<GetRatingByIdQuery, CollectionRatingResponseDto>
{
    private readonly WordsDbContext _dbContext;

    public GetRatingByIdQueryHandler(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CollectionRatingResponseDto> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
    {
        var rating = await _dbContext.WordCollectionRatings.FindAsync(request.Id);
        if (rating is null)
        {
            throw new NotFoundException("Invalid id");
        }

        return rating.Adapt<CollectionRatingResponseDto>();
    }
}