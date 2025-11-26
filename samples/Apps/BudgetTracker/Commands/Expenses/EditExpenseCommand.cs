using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Expenses;

internal static class EditExpenseCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Iter(s => s.ExpenseRepo
                         .GetEntityById(s.Console.GetId())
                         .Pipe(result => s.Console.DisplayResult(result, Constants.Edit.GetSuccessMessage))
                         .Pipe(result => s.PerformEdit(result)))
             .Map(s => s with { Command = metadata.Name });

    private static void PerformEdit(this AppState state, Result<Expense> editExpense) =>
        editExpense.Iter(v => v.UpdateExpense(
                                    state.Console.GetName(v.Name),
                                    state.Console.GetCategoryId(state.CategoryRepo, v.CategoryId),
                                    state.Console.GetCommittedDate(v.CommittedDate),
                                    state.Console.GetActual(v.Actual)))
                    .Map(state.ExpenseRepo.Update)
                    .Iter(result => state.Console.DisplayResult(result, Constants.Edit.SuccessMessage));

    private static int GetId(this IAnsiConsole console) => console.Prompt(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt(new TextPrompt<string>(Constants.Edit.NameLabel).DefaultValue(prevName));

    private static int GetCategoryId(this IAnsiConsole console, ICategoryRepository catRepo, int prevCatId) =>
        console.GetExistingCategoryId(Constants.Edit.CategoryIdLabel, catRepo, prevCatId);

    private static DateTimeOffset GetCommittedDate(this IAnsiConsole console, DateTimeOffset prevDate) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Edit.DateLabel, prevDate);

    private static decimal GetActual(this IAnsiConsole console, decimal prevActual) =>
        CurrencyComponent.Input(console, Constants.Edit.ActualLabel, prevActual, false);
}
