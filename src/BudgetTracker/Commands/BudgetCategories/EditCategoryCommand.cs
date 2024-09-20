using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class EditCategoryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Map(s => s.CategoryRepo.GetEntityById(s.Console.GetId())
                 .Apply(result => s.Console.DisplayMaybe(result, Constants.Edit.GetSuccessMessage))
                 .Apply(result => PerformEdit(s.Console, s.CategoryRepo, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformEdit(
        IAnsiConsole console,
        ICategoryRepository repo,
        Maybe<BudgetCategory> editCategory) =>
            editCategory.OnSomething(
                v => v.Apply(v => v.UpdateCategory(console.GetName(v.Name), console.GetBudgetAmount(v.BudgetedAmount)))
                      .Map(entry => repo.Update(entry))
                      .Apply(result => console.DisplayMaybe(result, Constants.Edit.SuccessMessage))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt(new TextPrompt<string>(Constants.Edit.NameLabel)
                            .DefaultValue(prevName));

    private static decimal GetBudgetAmount(this IAnsiConsole console, decimal prevAmount) =>
        MoneyComponent.Input(console, Constants.Add.AmountLabel, prevAmount);
}
