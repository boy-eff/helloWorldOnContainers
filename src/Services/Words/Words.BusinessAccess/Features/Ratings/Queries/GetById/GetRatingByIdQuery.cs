using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.Features.Ratings.Queries.GetById;

public record GetRatingByIdQuery(int Id) : IRequest<CollectionRatingResponseDto>;