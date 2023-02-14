using AuthService.Application.Dtos;
using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto);
        Task<ServiceResult<List<AppUserDto>>> GetUsersAsync(AppUserDto appUserDto);
        Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id);
        Task<ServiceResult<int>> UpdateUserAsync(AppUserDto appUserDto);
        Task<ServiceResult<int>> DeleteUserAsync(int id);
    }
}