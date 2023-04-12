using MediatR;
using Microsoft.AspNetCore.Http;

namespace Words.BusinessAccess.MediatR.Features.Users.Commands.UpdatePhoto;

public record UpdateUserPhotoCommand(IFormFile File) : IRequest<Unit>;