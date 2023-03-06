using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.Features.Ratings.Commands.Update;

public record UpdateRatingCommand(int Id, CollectionRatingUpdateDto RatingDto) : IRequest<CollectionRatingResponseDto>;