using Apps.Common;
using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ShowMonthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.SnapshotRepo.GetSnapshotForMonth(state.Console.GetMonth())
            .Apply(maybe => state.Console.DisplayMaybe(maybe, 
                snapshot => snapshot
                    .Apply(s => state.Console.DisplayHeader(Constants.Month.HeaderLabel(s.StartDate)))
                    .Apply(s => state.Console.Write(SnapshotTableHelper.CreateIncomeTable(s)))
                    .Apply(s => state.Console.Write(SnapshotTableHelper.CreateExpenseTable(s)))))
            .Map(m => state with { Command = metadata.Name });

    private static DateTimeOffset GetMonth(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Month.DateLabel).StartOfMonth();
}
