namespace WealthTracker.Common;

internal static class ListExtensions
{
    public static string AsString<T>(this List<T> list, string defaultMessage = "") =>
        list.Count > 0 ? string.Join(", ", list) : defaultMessage;

    public static string AsString<T>(this T[] array, string defaultMessage = "") =>
        array.Length > 0 ? string.Join(", ", array) : defaultMessage;
}
