using Shared.Enums;

namespace Shared.Messages;

public class UserCreatedMessage
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public EnglishLevel EnglishLevel { get; set; }
}