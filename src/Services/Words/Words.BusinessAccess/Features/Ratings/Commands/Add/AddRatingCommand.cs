using MediatR;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.CollectionRating;

namespace Words.BusinessAccess.Features.Ratings.Commands.Add;

public record AddRatingCommand(CollectionRatingCreateDto CreateDto) : IRequest<CollectionRatingResponseDto>;