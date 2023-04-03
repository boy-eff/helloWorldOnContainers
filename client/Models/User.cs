using HelloWorldClient.Enums;

namespace HelloWorldClient.Models;

public class User
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
}