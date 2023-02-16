using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
        
    /// <summary>
    /// Register new user
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     POST /users
    ///     {
    ///        "userName": "username",
    ///        "password": "password"
    ///     }
    ///     
    /// </remarks>
    /// <response code="200">Returns the newly created item</response>
    /// <response code="400">If the username or password are invalid or user with specified username already exists</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserAsync(AppUserRegisterDto appUserRegisterDto)
    {
        var serviceResult = await _userService.AddUserAsync(appUserRegisterDto);
        if (!serviceResult.Succeeded)
        {
            return BadRequest(serviceResult.Errors[0].Message);
        }
        return Ok(serviceResult.Value);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <response code="200">Returns all users</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsersAsync()
    {
        var serviceResult = await _userService.GetUsersAsync();
        if (!serviceResult.Succeeded)
        {
            return BadRequest(serviceResult.Errors[0].Message);
        }
        return Ok(serviceResult.Value);
    }
        
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <response code="200">Returns user with specified id</response>
    /// <response code="404">If the user is not found</response>
    /// <response code="400">If something went wrong</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var serviceResult = await _userService.GetUserByIdAsync(id);
        if (serviceResult.Succeeded) return Ok(serviceResult.Value);
        if (serviceResult.Errors[0].StatusCode == ServiceErrorStatusCode.NotFound)
        {
            return NotFound(serviceResult.Errors[0].Message);
        }
        return BadRequest(serviceResult.Errors[0].Message);
    }
}