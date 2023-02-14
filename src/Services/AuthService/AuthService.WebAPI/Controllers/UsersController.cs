using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using AuthService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(AppUserRegisterDto appUserRegisterDto)
        {
            var serviceResult = await _userService.AddUserAsync(appUserRegisterDto);
            if (!serviceResult.Succeeded)
            {
                return BadRequest(serviceResult.Errors[0].Message);
            }
            return Ok(serviceResult.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var serviceResult = await _userService.GetUsersAsync();
            if (!serviceResult.Succeeded)
            {
                return BadRequest(serviceResult.Errors[0].Message);
            }
            return Ok(serviceResult.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var serviceResult = await _userService.GetUserByIdAsync(id);
            if (!serviceResult.Succeeded)
            {
                if (serviceResult.Errors[0].StatusCode == ServiceErrorStatusCode.NotFound)
                return NotFound(serviceResult.Errors[0].Message);
            }
            return Ok(serviceResult.Value);
        }
    }
}