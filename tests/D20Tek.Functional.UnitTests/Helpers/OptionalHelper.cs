namespace D20Tek.Functional.UnitTests.Helpers;

internal static class OptionalHelper
{
    public static Optional<int> TryParse(string text) =>
        int.TryParse(text, out int parsed)
            ? Optional<int>.Some(parsed)
            : Optional<int>.None();

    public static Task<Optional<int>> TryParseAsync(string text) =>
        Task.FromResult(int.TryParse(text, out int parsed)
                           ? Optional<int>.Some(parsed)
                           : Optional<int>.None());
}
