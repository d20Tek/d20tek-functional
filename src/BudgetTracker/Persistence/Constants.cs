using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal static class Constants
{
    public const string DatabaseFile = "budget-data.json";
    public const string AppFolder = "\\d20tek-fin";

    public static Maybe<T> NotFoundError<T>(int id) =>
        new Failure<T>(Error.NotFound("Entity.NotFound", $"Entity with id={id} not found."));

    public static Maybe<int> AlreadyExistsError(int id) =>
        new Failure<int>(Error.Conflict("Entry.AlreadyExists", $"Entry with id={id} already exists."));
}
