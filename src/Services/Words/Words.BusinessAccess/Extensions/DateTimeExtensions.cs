namespace Words.BusinessAccess.Extensions;

public static class DateTimeExtensions
{
    private const int _marchMonthNumber = 3;
    private const int _firstDayInMonth = 1;
    private const int _februaryMonthNumber = 2;
    private const int _februaryLastDayInMonthLeap = 29;
    
    public static bool IsFirstOfMarch(this DateTime dateTime)
    {
        return dateTime.Day == _firstDayInMonth && dateTime.Month == _marchMonthNumber;
    }
    
    public static bool IsLastDayOfLeapFebruary(this DateTimeOffset dateTime)
    {
        return dateTime.Day == _februaryLastDayInMonthLeap && dateTime.Month == _februaryMonthNumber;
    }
}