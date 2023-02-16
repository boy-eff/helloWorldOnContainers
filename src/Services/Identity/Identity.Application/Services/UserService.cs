using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto)
    {
        var appUser = appUserDto.Adapt<AppUser>();

        var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);

        if(!identityResult.Succeeded)
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
            return new ServiceResult<AppUserDto>()
            {
                Errors = new List<ServiceError>
                {
                    new ServiceError(ServiceErrorStatusCode.NotFound, "User not found")
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