using Identity.Application.Dtos;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUserService
{
    Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto);
    Task<ServiceResult<List<AppUserDto>>> GetUsersAsync();
    Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id);
    Task<ServiceResult<int>> AddUserToRoleAsync(int roleId, int userId);
    Task<ServiceResult<int>> ChangePasswordAsync(string oldPassword, string newPassword);
}