using Achievements.Application.Contracts;
using Achievements.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Achievements.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly IUsersAchievementsService _usersAchievementsService;

    public UsersController(IUsersAchievementsService usersAchievementsService)
    {
        _usersAchievementsService = usersAchievementsService;
    }

    /// <summary>
    /// Get user achievements
    /// </summary>
    /// <response code="200">Returns list of achievements</response>
    /// <response code="401">If user is not authenticated</response>
    /// <response code="404">If achievement is not found</response>
    [HttpGet("{id:int}/achievements")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UsersAchievementsDto>>> GetByCollectionIdAsync(int id)
    {
        var achievements = await _usersAchievementsService.GetUserAchievementsByIdAsync(id);
        return Ok(achievements);
    }
}