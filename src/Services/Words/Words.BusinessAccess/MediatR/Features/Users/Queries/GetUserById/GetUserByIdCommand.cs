using MediatR;
using Words.BusinessAccess.Dtos.User;

namespace Words.BusinessAccess.MediatR.Features.Users.Queries.GetUserById;

public record GetUserByIdCommand(int UserId) : IRequest<UserResponseDto>;