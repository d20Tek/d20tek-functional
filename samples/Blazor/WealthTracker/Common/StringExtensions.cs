namespace WealthTracker.Common;

internal static class StringExtensions
{
    public static bool HasText(this string text) => !string.IsNullOrEmpty(text);

    public static bool IsEmpty(this string text) => string.IsNullOrEmpty(text);
}
