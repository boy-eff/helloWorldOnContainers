namespace Words.BusinessAccess.Extensions;

public static class DateTimeExtensions
{
    private static int _aprilMonthNumber = 3;
    private static int _firstDayInMonth = 1;
    private static int _februaryMonthNumber = 2;
    private static int _februaryLastDayInMonth = 29;
    
    public static bool IsFirstOfApril(this DateTime dateTime)
    {
        return dateTime.Day == _firstDayInMonth && dateTime.Month == _aprilMonthNumber;
    }
    
    public static bool IsLastOfLeapFebruary(this DateTimeOffset dateTime)
    {
        return dateTime.Day == _februaryLastDayInMonth && dateTime.Month == _februaryMonthNumber;
    }
}