using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using AuthService.Domain.Data.Entities;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Services
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto)
        {
            var result = new ServiceResult<int>();
            var appUser = appUserDto.Adapt<AppUser>();

            var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);

            if(!identityResult.Succeeded)
            {
                result.Errors.Add(
                    new ServiceError(ServiceErrorStatusCode.WrongAction,
                    identityResult.Errors.ToList()[0].Description)
                );
            }
            result.Value = appUser.Id;
            return result;
        }

        public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id)
        {
            var result = new ServiceResult<AppUserDto>();
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                var error = new ServiceError(ServiceErrorStatusCode.NotFound, "User not found");
                result.Errors.Add(error);
                return result;
            }
            var userDto = user.Adapt<AppUserDto>();
            result.Value = userDto;
            return result;
        }

        public async Task<ServiceResult<List<AppUserDto>>> GetUsersAsync()
        {
            var result = new ServiceResult<List<AppUserDto>>();
            var users = await _userManager.Users.ToListAsync();
            var usersDto = users.Adapt<List<AppUserDto>>();
            result.Value = usersDto;
            return result;
        }
    }
}