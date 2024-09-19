using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class AddCategoryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Map(s => new BudgetCategory(0, state.Console.GetName(), state.Console.GetBudgetAmount())
                .Map(entry => s.CategoryRepo.Create(entry))
                .Apply(result => s.Console.DisplayMaybe(result, Constants.Add.SuccessMessage))
                .Map(_ => s with { Command = metadata.Name }));

    private static string GetName(this IAnsiConsole console) =>
        console.Prompt<string>(new TextPrompt<string>(Constants.Add.NameLabel));

    private static decimal GetBudgetAmount(this IAnsiConsole console) =>
        MoneyComponent.Input(console, Constants.Add.AmountLabel);
}
