using MediatR;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Queries.GetById;

public record GetRatingByIdQuery(int Id) : IRequest<CollectionRatingResponseDto>;