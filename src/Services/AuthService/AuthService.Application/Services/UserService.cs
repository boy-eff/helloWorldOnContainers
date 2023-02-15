using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto)
    {
        var errors = new List<ServiceError>();
        var appUser = appUserDto.Adapt<AppUser>();

        var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);

        if(!identityResult.Succeeded)
        {
            errors.Add(
                new ServiceError(ServiceErrorStatusCode.WrongAction,
                    identityResult.Errors.ToList()[0].Description)
            );
        }

        return new ServiceResult<int>()
        {
            Errors = errors
        };
    }

    public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id)
    {
        var errors = new List<ServiceError>();
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            var error = new ServiceError(ServiceErrorStatusCode.NotFound, "User not found");
            errors.Add(error);
            return new ServiceResult<AppUserDto>()
            {
                Errors = errors
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