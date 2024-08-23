namespace MartianTrail.Common;

internal static class DictionaryExtensions
{
    public static Func<TK, TV?> ToLookupWithDefault<TK, TV>(this IDictionary<TK, TV> source) =>
        x => source.ContainsKey(x) ? source[x] : default;
}
