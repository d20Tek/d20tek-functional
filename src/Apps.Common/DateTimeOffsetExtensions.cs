namespace Apps.Common;

internal static class DateTimeOffsetExtensions
{
    public static string ToMonthDay(this DateTimeOffset d) => d.ToString("MM-dd");

    public static string ToDateString(this DateTimeOffset d) => d.ToString("MM-dd-yyyy");

    public static DateTimeOffset StartOfMonth(this DateTimeOffset d) =>
    new(d.Year, d.Month, 1, 0, 0, 0, d.Offset);

    public static DateTimeOffset StartOfYear(this DateTimeOffset d) =>
    new(d.Year, 1, 1, 0, 0, 0, d.Offset);

    public static bool IsFutureDate(this DateTimeOffset d) => d > DateTimeOffset.Now;
}
