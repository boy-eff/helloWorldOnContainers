using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using AuthService.Domain.Data.Entities;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using Mapster;
using Microsoft.AspNetCore.Identity;

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

        public Task<ServiceResult<int>> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<List<AppUserDto>>> GetUsersAsync(AppUserDto appUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> UpdateUserAsync(AppUserDto appUserDto)
        {
            throw new NotImplementedException();
        }
    }
}