using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Identity.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<AppUser> userManager, IPublishEndpoint publishEndpoint, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto)
    {
        var appUser = appUserDto.Adapt<AppUser>();

        var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);
        if (!identityResult.Succeeded)
        {
            _logger.LogInformation("Failed to create user. Error code: {ErrorCode} ", identityResult.Errors.First().Code);
            return new ServiceResult<int>()
            {
                Value = 0,
                Errors = new List<ServiceError>
                {
                    new(ServiceErrorStatusCode.WrongAction,
                        identityResult.Errors.ToList()[0].Description)
                }
            };
        }
        _logger.LogInformation("User with id {Id} was successfully created", appUser.Id);

        var message = appUser.Adapt<UserCreatedMessage>();

        await _publishEndpoint.Publish(message);


        return new ServiceResult<int>()
        {
            Value = appUser.Id
        };
    }

    public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            _logger.LogInformation("User with id {Id} was not found", id);
            return new ServiceResult<AppUserDto>()
            {
                Errors = new List<ServiceError>
                {
                    new ServiceError(ServiceErrorStatusCode.NotFound, "User was not found")
                }
            };
        }
        _logger.LogInformation("User with id {Id} was successfully retrieved", id);
        var userDto = user.Adapt<AppUserDto>();
        return new ServiceResult<AppUserDto>()
        {
            Value = userDto
        };
    }

    public async Task<ServiceResult<List<AppUserDto>>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersDto = users.Adapt<List<AppUserDto>>();
        _logger.LogInformation("Users was successfully retrieved");
        return new ServiceResult<List<AppUserDto>>()
        {
            Value = usersDto
        };
    }
}