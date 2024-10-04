using BudgetTracker.Commands.BudgetCategories;
using BudgetTracker.Commands.Incomes;
using BudgetTracker.Commands.Expenses;
using BudgetTracker.Commands.Show;

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

        new ("list-inc", ["list-income", "list-inc", "li"], ListIncomeCommand.Handle),
        new ("add-inc", ["add-income", "add-inc", "ai"], AddIncomeCommand.Handle),
        new ("edit-inc", ["edit-income", "edit-inc", "ei"], EditIncomeCommand.Handle),
        new ("del-inc", ["delete-income", "del-inc", "di"],
            (s, m) => CommonHandlers.DeleteFromRepository(s, m, Incomes.Constants.Delete.Header, id => s.IncomeRepo.Delete(id))),

        new ("current", ["current", "c"], ShowCurrentCommand.Handle),
        new ("reconcile", ["reconcile", "rec", "r"], ReconcileMonthCommand.Handle),
        new ("list-months", ["list-months", "lm"], ListSnapshotsCommand.Handle),
        new ("month", ["month", "show-month", "m"], ShowMonthCommand.Handle),
        new ("year", ["year", "show-year", "y"], ShowYearCommand.Handle),

        new ("help", ["help", "h"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit)
    ];

    public static string[] GetCommands() =>
        GetCommandTypes().SelectMany(x => x.AllowedCommands).ToArray();

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
