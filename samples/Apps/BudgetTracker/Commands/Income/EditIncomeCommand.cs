using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Incomes;

internal static class EditIncomeCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Iter(s => s.IncomeRepo
                         .GetEntityById(s.Console.GetId())
                         .Pipe(result => s.Console.DisplayResult(result, Constants.Edit.GetSuccessMessage))
                         .Pipe(result => s.PerformEdit(result)))
             .Map(s => s with { Command = metadata.Name });

    private static void PerformEdit(this AppState state, Result<Income> editIncome) =>
            editIncome.Iter(v => v.UpdateIncome(
                                   state.Console.GetName(v.Name),
                                   state.Console.GetDepositDate(v.DepositDate),
                                   state.Console.GetAmount(v.Amount)))
                      .Map(state.IncomeRepo.Update)
                      .Iter(result => state.Console.DisplayResult(result, Constants.Edit.SuccessMessage));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt(new TextPrompt<string>(Constants.Edit.NameLabel).DefaultValue(prevName));

    private static DateTimeOffset GetDepositDate(this IAnsiConsole console, DateTimeOffset prevDate) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Edit.DateLabel, prevDate);

    private static decimal GetAmount(this IAnsiConsole console, decimal prevAmount) =>
        CurrencyComponent.Input(console, Constants.Edit.AmountLabel, prevAmount, false);
}
