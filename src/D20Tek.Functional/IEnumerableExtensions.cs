namespace D20Tek.Functional;

public static class IEnumerableExtensions
{
    public static void ForEach<TIn>(this IEnumerable<TIn> enumerable, Action<TIn> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}
