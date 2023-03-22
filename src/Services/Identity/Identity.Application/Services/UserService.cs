using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Messages;

namespace Identity.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAuthDbContext _authDbContext;

    public UserService(UserManager<AppUser> userManager, IPublishEndpoint publishEndpoint, IAuthDbContext authDbContext)
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
        _authDbContext = authDbContext;
    }

    public async Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto)
    {
        var appUser = appUserDto.Adapt<AppUser>();
        
        await using var transaction = await _authDbContext.Database.BeginTransactionAsync();
        try
        {
            var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);
            if (!identityResult.Succeeded)
            {
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

            var message = appUser.Adapt<UserCreatedMessage>();

            await _publishEndpoint.Publish(message);

            await _authDbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ServiceResult<int>()
            {
                Value = appUser.Id
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return new ServiceResult<AppUserDto>()
            {
                Errors = new List<ServiceError>
                {
                    new ServiceError(ServiceErrorStatusCode.NotFound, "User is not found")
                }
            };
        }
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
        return new ServiceResult<List<AppUserDto>>()
        {
            Value = usersDto
        };
    }
}