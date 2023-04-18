using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.Dtos.User;
using Words.BusinessAccess.MediatR.Features.Users.Commands.UpdatePhoto;
using Words.BusinessAccess.MediatR.Features.Users.Queries.GetUserById;

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
    public async Task<ActionResult> UpdateUserPhotoAsync(IFormFile file)
    {
        var command = new UpdateUserPhotoCommand(file);
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserResponseDto>> GetUserByIdAsync(int userId)
    {
        var query = new GetUserByIdCommand(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}