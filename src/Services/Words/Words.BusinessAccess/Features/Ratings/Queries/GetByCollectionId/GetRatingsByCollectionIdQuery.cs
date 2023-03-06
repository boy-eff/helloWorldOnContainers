using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.Features.Ratings.Queries.GetByCollectionId;

public record GetRatingsByCollectionIdQuery(int CollectionId) : IRequest<IEnumerable<CollectionRatingResponseDto>>;