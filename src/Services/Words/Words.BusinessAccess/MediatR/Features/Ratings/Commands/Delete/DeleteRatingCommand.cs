using MediatR;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Delete;

public record DeleteRatingCommand(int Id) : IRequest<int>;