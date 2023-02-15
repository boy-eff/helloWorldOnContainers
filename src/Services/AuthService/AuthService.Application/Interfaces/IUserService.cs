using AuthService.Application.Dtos;
using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces;

public interface IUserService
{
    Task<ServiceResult<int>> AddUserAsync(AppUserRegisterDto appUserDto);
    Task<ServiceResult<List<AppUserDto>>> GetUsersAsync();
    Task<ServiceResult<AppUserDto>> GetUserByIdAsync(int id);
}