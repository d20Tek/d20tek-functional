namespace D20Tek.Functional.UnitTests.Helpers;

internal class ResultHelper
{
    public static Result<int> TryParse(string text) =>
        int.TryParse(text, out int parsed)
            ? Result<int>.Success(parsed)
            : Result<int>.Failure(Error.Validation("Parse.Failed", "Couldn't parse input text."));

    public static Task<Result<int>> TryParseAsync(string text) =>
        Task.FromResult(TryParse(text));
}
