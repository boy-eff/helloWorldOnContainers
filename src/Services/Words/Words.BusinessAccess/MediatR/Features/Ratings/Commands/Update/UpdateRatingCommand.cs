using MediatR;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Update;

public record UpdateRatingCommand(int Id, CollectionRatingRequestDto RatingDto) : IRequest<CollectionRatingResponseDto>;