namespace WealthTracker.Persistence;

internal static class Constants
{
    public const string DatabaseFile = "wealth-data.json";
    public const string AppFolder = "\\WealthTracker";

    public static Exception NotFoundError(int id) =>
        new InvalidOperationException($"Entry with id={id} not found.");

    public static Exception AlreadyExistsError(int id) =>
        new InvalidOperationException($"Entry with id={id} already exists.");
}
