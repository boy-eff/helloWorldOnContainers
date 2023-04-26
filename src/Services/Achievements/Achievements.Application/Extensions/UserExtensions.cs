using Achievements.Domain.Models;

namespace Achievements.Application.Extensions;

public static class UserExtensions
{
    public static void AddBalanceAndExperience(this User user, int balance, int experience)
    {
        user.Balance += balance;
        user.Experience += experience;
    }
}