namespace D20Tek.Functional.UnitTests.Helpers;

internal static class OptionHelper
{
    public static Option<int> TryParse(string text) =>
        int.TryParse(text, out int parsed)
            ? Option<int>.Some(parsed)
            : Option<int>.None();
}
