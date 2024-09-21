using BudgetTracker.Commands.BudgetCategories;
using BudgetTracker.Commands.Credits;
using BudgetTracker.Commands.Expenses;

namespace BudgetTracker.Commands;

internal sealed class Configuration
{
    public static Func<CommandTypeMetadata[]> GetCommandTypes = () =>
    [
        new ("list-cat", ["list-categories", "list-cat", "lc"], ListCategoriesCommand.Handle),
        new ("add-cat", ["add-categories", "add-cat", "ac"], AddCategoryCommand.Handle),
        new ("edit-cat", ["edit-category", "edit-cat", "ec"], EditCategoryCommand.Handle),
        new ("delete-cat", ["delete-category", "del-cat", "dc"],
            (s, m) => CommonHandlers.DeleteFromRepository(s, m, BudgetCategories.Constants.Delete.Header, id => s.CategoryRepo.Delete(id))),

        new ("list-exp", ["list-expenses", "list-exp", "le"], ListExpenseCommand.Handle),
        new ("add-exp", ["add-expense", "add-exp", "ae"], AddExpenseCommand.Handle),
        new ("edit-exp", ["edit-expense", "edit-exp", "ee"], EditExpenseCommand.Handle),
        new ("del-exp", ["delete-expense", "del-exp", "de"],
            (s, m) => CommonHandlers.DeleteFromRepository(s, m, Expenses.Constants.Delete.Header, id => s.ExpenseRepo.Delete(id))),

        new ("list-cred", ["list-credits", "list-cred", "lcr"], ListCreditsCommand.Handle),
        new ("add-cred", ["add-credit", "add-cred", "acr"], AddCreditCommand.Handle),

        new ("help", ["help", "h"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit)
    ];

    public static string[] GetCommands() =>
        GetCommandTypes().SelectMany(x => x.AllowedCommands).ToArray();

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
