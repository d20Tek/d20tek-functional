using Apps.Common;
using BudgetTracker.Entities;
using BudgetTracker.Persistence;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ShowMonthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.SnapshotRepo.GetSnapshotForMonth(s.Console.GetMonth())
                 .Pipe(result => s.Console.DisplayResult(
                     result,
                     snapshot => s.Console.DisplayReconciledMonth(snapshot))))
             .Map(s => s with { Command = metadata.Name });

    private static DateTimeOffset GetMonth(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Month.DateLabel).StartOfMonth();

    private static void DisplayReconciledMonth(this IAnsiConsole console, Identity<ReconciledSnapshot> snapshot) =>
        snapshot.Iter(s => console.DisplayHeader(Constants.Month.HeaderLabel(s.StartDate)))
                .Iter(s => console.Write(SnapshotTableHelper.CreateIncomeTable(s)))
                .Iter(s => console.Write(SnapshotTableHelper.CreateExpenseTable(s)));
}
