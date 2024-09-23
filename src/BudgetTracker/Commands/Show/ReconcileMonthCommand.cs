using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ReconcileMonthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Console.GetMonth()
            .Map(month => 
                ReconciledBuilder.GenerateSnapshot(
                    month.Start,
                    month.End,
                    state.IncomeRepo,
                    state.CategoryRepo,
                    state.ExpenseRepo)
                       .Map(r => state with { Command = metadata.Name }));

    public static (DateTimeOffset Start, DateTimeOffset End) GetMonth(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Reconcile.DateLabel, DateTimeOffset.Now.StartOfMonth())
            .StartOfMonth()
            .Map(start => (start, start.AddMonths(1)));
}
