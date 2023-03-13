using MediatR;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Queries.GetByCollectionId;

public record GetRatingsByCollectionIdQuery(int CollectionId) : IRequest<IEnumerable<CollectionRatingResponseDto>>;