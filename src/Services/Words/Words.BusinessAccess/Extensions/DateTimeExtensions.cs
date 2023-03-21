namespace Words.BusinessAccess.Extensions;

public static class DateTimeExtensions
{
    private const int MarchMonthNumber = 3;
    private const int FirstDayInMonth = 1;
    private const int FebruaryMonthNumber = 2;
    private const int FebruaryLastDayInMonthLeap = 29;
    
    public static bool IsFirstOfMarch(this DateTime dateTime)
    {
        return dateTime.Day == FirstDayInMonth && dateTime.Month == MarchMonthNumber;
    }
    
    public static bool IsLastDayOfLeapFebruary(this DateTimeOffset dateTime)
    {
        return dateTime.Day == FebruaryLastDayInMonthLeap && dateTime.Month == FebruaryMonthNumber;
    }
}