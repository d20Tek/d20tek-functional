using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class EditCategoryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Iter(s => s.CategoryRepo
                         .GetEntityById(s.Console.GetId())
                         .Pipe(result => s.Console.DisplayResult(result, Constants.Edit.GetSuccessMessage))
                         .Pipe(result => PerformEdit(s.Console, s.CategoryRepo, result)))
             .Map(s => s with { Command = metadata.Name });

    private static void PerformEdit(
        IAnsiConsole console,
        ICategoryRepository repo,
        Result<BudgetCategory> editCategory) =>
            editCategory.Iter(v => v.UpdateCategory(console.GetName(v.Name), console.GetBudgetAmount(v.BudgetedAmount)))
                        .Map(entry => repo.Update(entry))
                        .Iter(result => console.DisplayResult(result, Constants.Edit.SuccessMessage));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt(new TextPrompt<string>(Constants.Edit.NameLabel)
                            .DefaultValue(prevName));

    private static decimal GetBudgetAmount(this IAnsiConsole console, decimal prevAmount) =>
        CurrencyComponent.Input(console, Constants.Edit.AmountLabel, prevAmount, false);
}
