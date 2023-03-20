using Achievements.Domain;
using Achievements.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Achievements.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/achievements")]
public class AchievementsController: ControllerBase
{
    /// <summary>
    /// Get achievements
    /// </summary>
    /// <response code="200">Returns achievements</response>
    /// <response code="401">If user is not authenticated</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IEnumerable<Achievement>> Get()
    {
        return Ok(SeedData.Achievements);
    }
}