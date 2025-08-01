namespace D20Tek.Functional.UnitTests.Helpers;

[Obsolete("Deprecated - moving to Optional<T> instead.")]
internal static class OptionHelper
{
    public static Option<int> TryParse(string text) =>
        int.TryParse(text, out int parsed)
            ? Option<int>.Some(parsed)
            : Option<int>.None();

    public static Task<Option<int>> TryParseAsync(string text) =>
        Task.FromResult(int.TryParse(text, out int parsed)
                           ? Option<int>.Some(parsed)
                           : Option<int>.None());
}
