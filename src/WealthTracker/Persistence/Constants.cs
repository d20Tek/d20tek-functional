using D20Tek.Minimal.Functional;

namespace WealthTracker.Persistence;

internal static class Constants
{
    public const string DatabaseFile = "wealth-data.json";
    public const string AppFolder = "\\WealthTracker";

    public static Maybe<WealthDataEntry> NotFoundError(int id) =>
        new Failure<WealthDataEntry>(Error.NotFound("Entry.NotFound", $"Entry with id={id} not found."));

    public static Exception AlreadyExistsError(int id) =>
        new InvalidOperationException($"Entry with id={id} already exists.");
}
