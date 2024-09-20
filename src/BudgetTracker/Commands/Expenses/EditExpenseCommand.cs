using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Expenses;

internal static class EditExpenseCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Map(s => s.ExpenseRepo.GetEntityById(s.Console.GetId())
                 .Apply(result => s.Console.DisplayMaybe(result, Constants.Edit.GetSuccessMessage))
                 .Apply(result => PerformEdit(s, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformEdit(AppState state, Maybe<Expense> editExpense) =>
            editExpense.OnSomething(
                v => v.Apply(v => v.UpdateExpense(
                    state.Console.GetName(v.Name),
                    state.Console.GetCategoryId(state.CategoryRepo, v.CategoryId),
                    state.Console.GetCommittedDate(v.CommittedDate),
                    state.Console.GetActual(v.Actual)))
                      .Map(entry => state.ExpenseRepo.Update(entry))
                      .Apply(result => state.Console.DisplayMaybe(result, Constants.Edit.SuccessMessage))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt(new TextPrompt<string>(Constants.Edit.NameLabel)
                            .DefaultValue(prevName));

    private static int GetCategoryId(this IAnsiConsole console, ICategoryRepository catRepo, int prevCatId) =>
        console.Prompt(new TextPrompt<int>(Constants.Edit.CategoryIdLabel)
                            .DefaultValue(prevCatId)
                            .Validate(
                                x => catRepo.GetEntityById(x) is Something<BudgetCategory>,
                                Constants.Edit.InvalidCatIdError));

    private static DateTimeOffset GetCommittedDate(this IAnsiConsole console, DateTimeOffset prevDate) =>
        console.Prompt(new TextPrompt<DateTimeOffset>(Constants.Edit.DateLabel)
                            .DefaultValue(prevDate)
                            .WithConverter(date => date.ToDateString()));

    private static decimal GetActual(this IAnsiConsole console, decimal prevActual) =>
        CurrencyComponent.Input(console, Constants.Edit.ActualLabel, prevActual, false);
}

// todo: create new CurrencyComponent to encapsulate logic for getting currency from input prompt.
// todo: move GetActual, GetCommittedDate, and GetCategoryId into reuseable extension methods.
// todo: refactor inputs on BudgetCategory Add/Edit as well.
// todo: move delete command code into shared code too.
// todo: add Income item CRUD operations.
// todo: implement show operation that shows the current budgeted versus actuals in a table.
// todo: implement close out month operation to snapshot budget and expenses for a particular month... save closed months to separate archive file.
// todo: implement show operation on past closed month data.
// todo: implement show year operation as well.
