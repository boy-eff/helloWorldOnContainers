namespace Words.BusinessAccess.Extensions;

public static class DateTimeExtensions
{
    private const int s_marchMonthNumber = 3;
    private const int s_firstDayInMonth = 1;
    private const int s_februaryMonthNumber = 2;
    private const int s_februaryLastDayInMonthLeap = 29;
    
    public static bool IsFirstOfMarch(this DateTime dateTime)
    {
        return dateTime.Day == s_firstDayInMonth && dateTime.Month == s_marchMonthNumber;
    }
    
    public static bool IsLastDayOfLeapFebruary(this DateTimeOffset dateTime)
    {
        return dateTime.Day == s_februaryLastDayInMonthLeap && dateTime.Month == s_februaryMonthNumber;
    }
}