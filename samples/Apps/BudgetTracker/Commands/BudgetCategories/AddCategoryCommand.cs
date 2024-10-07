using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class AddCategoryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Iter(s => s.CategoryRepo
                            .Create(s.CreateEntity())
                            .Pipe(result => s.Console.DisplayResult(result, e => Constants.Delete.SuccessMessage(e))))
             .Map(_ => state with { Command = metadata.Name });

    private static BudgetCategory CreateEntity(this AppState state) =>
        new(0, state.Console.GetName(), state.Console.GetBudgetAmount());

    private static string GetName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.Add.NameLabel));

    private static decimal GetBudgetAmount(this IAnsiConsole console) =>
        CurrencyComponent.Input(console, Constants.Add.AmountLabel, false);
}
