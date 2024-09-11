namespace Apps.Common;

internal static class StringExtensions
{
    public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

    public static bool HasText(this string value) => !string.IsNullOrWhiteSpace(value);

    public enum Overflow
    {
        Wrap,
        Crop,
        Ellipsis
    }

    private const string _ellipsis = "...";
    private const int _ellipsisLength = 3;

    public static string CapOverflow(this string str, int maxLength, Overflow overflow = Overflow.Ellipsis) =>
        (str.Length > maxLength)
            ? str.HandleOverflowTypes(maxLength, overflow)
            : str;

    private static string HandleOverflowTypes(this string str, int maxLength, Overflow overflow) =>
        overflow switch
        {
            Overflow.Crop => str.Substring(0, maxLength),
            Overflow.Ellipsis => string.Concat(str.AsSpan(0, maxLength - _ellipsisLength), _ellipsis),
            _ => str
        };
}
