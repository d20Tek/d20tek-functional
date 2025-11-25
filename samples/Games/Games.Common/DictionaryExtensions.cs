namespace Games.Common;

internal static class DictionaryExtensions
{
    public static Func<TK, TV?> ToLookupWithDefault<TK, TV>(this IDictionary<TK, TV> source) =>
        x => source.TryGetValue(x, out var value) ? value : default;
}
