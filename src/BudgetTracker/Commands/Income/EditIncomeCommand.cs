using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Incomes;

internal static class EditIncomeCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Map(s => s.IncomeRepo.GetEntityById(s.Console.GetId())
                 .Apply(result => s.Console.DisplayMaybe(result, Constants.Edit.GetSuccessMessage))
                 .Apply(result => PerformEdit(s, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformEdit(AppState state, Maybe<Income> editIncome) =>
            editIncome.OnSomething(
                v => v.Apply(v => v.UpdateIncome(
                    state.Console.GetName(v.Name),
                    state.Console.GetDepositDate(v.DepositDate),
                    state.Console.GetAmount(v.Amount)))
                      .Map(entry => state.IncomeRepo.Update(entry))
                      .Apply(result => state.Console.DisplayMaybe(result, Constants.Edit.SuccessMessage))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt(new TextPrompt<string>(Constants.Edit.NameLabel)
                            .DefaultValue(prevName));

    private static DateTimeOffset GetDepositDate(this IAnsiConsole console, DateTimeOffset prevDate) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Edit.DateLabel, prevDate);

    private static decimal GetAmount(this IAnsiConsole console, decimal prevAmount) =>
        CurrencyComponent.Input(console, Constants.Edit.AmountLabel, prevAmount, false);
}
