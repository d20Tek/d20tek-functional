namespace D20Tek.Functional;

public static class IEnumerableExtensions
{
    public static string AsString<T>(this IEnumerable<T> list, string defaultMessage = "") =>
        list.Any() ? string.Join(", ", list) : defaultMessage;

    public static void ForEach<TIn>(this IEnumerable<TIn> enumerable, Action<TIn> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}
