using MediatR;

namespace Words.BusinessAccess.Features.Ratings.Commands.Delete;

public record DeleteRatingCommand(int Id) : IRequest<int>;