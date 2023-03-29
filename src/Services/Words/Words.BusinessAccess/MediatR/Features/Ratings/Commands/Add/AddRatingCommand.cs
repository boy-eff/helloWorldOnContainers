using MediatR;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Add;

public record AddRatingCommand(int WordCollectionId, CollectionRatingRequestDto RequestDto) : IRequest<CollectionRatingResponseDto>;