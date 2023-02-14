using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
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
    }
}