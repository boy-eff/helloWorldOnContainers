using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.MediatR.Features.Users.Commands.UpdatePhoto;

namespace Words.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> UpdateUserPhoto(IFormFile file)
    {
        var command = new UpdateUserPhotoCommand(file);
        await _mediator.Send(command);
        return Ok();
    }
}