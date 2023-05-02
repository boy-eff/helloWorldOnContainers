using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

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
        return HandleServiceResult(serviceResult);
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
        return HandleServiceResult(serviceResult);
    }
        
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <response code="200">Returns user with specified id</response>
    /// <response code="404">If the user is not found</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var serviceResult = await _userService.GetUserByIdAsync(id);
        return HandleServiceResult(serviceResult);
    }
    
    /// <summary>
    /// Add user to role
    /// </summary>
    /// <response code="200">Returns updated user id</response>
    /// <response code="400">If user is already assigned to role</response>
    /// <response code="404">If user or role was not found</response>
    [Authorize(Policies.AdminOnly)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [HttpPost("{userId:int}/roles/{roleId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserToRoleAsync([FromRoute] int roleId, [FromRoute] int userId)
    {
        var serviceResult = await _userService.AddUserToRoleAsync(roleId, userId);
        return HandleServiceResult(serviceResult);
    }

    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [HttpPost("password")]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangePasswordDto changePasswordDto)
    {
        var result = await _userService.ChangePasswordAsync(changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        return HandleServiceResult(result);
    }
    
    private IActionResult HandleServiceResult<T>(ServiceResult<T> serviceResult)
    {
        if (serviceResult.Succeeded)
        {
            return Ok(serviceResult.Value);
        }

        var error = serviceResult.Errors[0];
        return error.StatusCode switch
        {
            ServiceErrorStatusCode.NotFound => NotFound(error.Message),
            ServiceErrorStatusCode.WrongAction => BadRequest(error.Message),
            ServiceErrorStatusCode.ForbiddenAction => Forbid(error.Message),
            ServiceErrorStatusCode.Conflict => Conflict(error.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}