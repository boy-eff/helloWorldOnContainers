using Shared.Enums;

namespace Identity.Application.Dtos;

public class AppUserRegisterDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
}