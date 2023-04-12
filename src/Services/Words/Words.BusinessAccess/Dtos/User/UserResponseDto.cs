using Shared.Enums;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Dtos.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}