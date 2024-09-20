using BudgetTracker.Commands.BudgetCategories;
using BudgetTracker.Commands.Expenses;

namespace BudgetTracker.Commands;

internal sealed class Configuration
{
    public static Func<CommandTypeMetadata[]> GetCommandTypes = () =>
    [
        new ("list-cat", ["list-categories", "list-cat", "lc"], ListCategoriesCommand.Handle),
        new ("add-cat", ["add-categories", "add-cat", "ac"], AddCategoryCommand.Handle),
        new ("delete-cat", ["delete-category", "del-cat", "dc"], DeleteCategoryCommand.Handle),
        new ("edit-cat", ["edit-category", "edit-cat", "ec"], EditCategoryCommand.Handle),
        new ("list-exp", ["list-expenses", "list-exp", "le"], ListExpenseCommand.Handle),
        new ("help", ["help", "h"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit)
    ];

    public static string[] GetCommands() =>
        GetCommandTypes().SelectMany(x => x.AllowedCommands).ToArray();

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
