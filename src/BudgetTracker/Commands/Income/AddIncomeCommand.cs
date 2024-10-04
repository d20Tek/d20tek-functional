using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Incomes;

internal static class AddIncomeCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Iter(s => s.IncomeRepo
                            .Create(s.Console.GatherIncomeData())
                            .Pipe(result => s.Console.DisplayResult(result, Constants.Add.SuccessMessage)))
             .Map(_ => state with { Command = metadata.Name });

    private static Income GatherIncomeData(this IAnsiConsole console) =>
        new(0, console.GetName(), console.GetDepositDate(), console.GetAmount());

    private static string GetName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.Add.NameLabel));

    private static DateTimeOffset GetDepositDate(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Add.DateLabel, DateTimeOffset.Now);

    private static decimal GetAmount(this IAnsiConsole console) =>
        CurrencyComponent.Input(console, Constants.Add.AmountLabel, false);
}
