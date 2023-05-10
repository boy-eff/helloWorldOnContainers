using MediatR;
using Microsoft.AspNetCore.Http;

namespace Words.BusinessAccess.MediatR.Features.Users.Commands.UpdateImage;

public record UpdateUserImageCommand(IFormFile File) : IRequest<Unit>;