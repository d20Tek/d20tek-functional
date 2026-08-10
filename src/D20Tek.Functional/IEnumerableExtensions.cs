namespace D20Tek.Functional;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/> providing functional utilities.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Joins all elements into a comma-separated string, or returns <paramref name="defaultMessage"/> if the collection is empty.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="list">The collection to join.</param>
    /// <param name="defaultMessage">The string to return when the collection is empty.</param>
    public static string AsString<T>(this IEnumerable<T> list, string defaultMessage = "") =>
        list.Any() ? string.Join(Constants.ListSeparator, list) : defaultMessage;

    /// <summary>
    /// Executes <paramref name="action"/> for each element in the collection.
    /// A functional alternative to a foreach loop.
    /// </summary>
    /// <typeparam name="TIn">The element type.</typeparam>
    /// <param name="enumerable">The collection to iterate.</param>
    /// <param name="action">The action to execute on each element.</param>
    public static void ForEach<TIn>(this IEnumerable<TIn> enumerable, Action<TIn> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}
