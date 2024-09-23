using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ReconcileMonthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Console
            .Apply(c => c.DisplayHeader(Constants.Reconcile.HeaderLabel))
            .GetMonth()
            .Map(month => state.CreateReconciledSnapshot(month.Start, month.End))
            .Apply(r => PerformReconciliation(r, state.Console))
            .Map(r => state with { Command = metadata.Name });

    private static (DateTimeOffset Start, DateTimeOffset End) GetMonth(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Reconcile.DateLabel, DateTimeOffset.Now.StartOfMonth())
            .StartOfMonth()
            .Map(start => (start, start.AddMonths(1)));

    private static ReconciledSnapshot CreateReconciledSnapshot(
        this AppState state,
        DateTimeOffset start,
        DateTimeOffset end) =>
        ReconciledBuilder.GenerateSnapshot(
            start,
            end,
            state.IncomeRepo,
            state.CategoryRepo,
            state.ExpenseRepo)
                .Apply(r => state.Console.WriteMessage(Constants.Reconcile.ItemsToReconcile(start.ToString("MMM"))))
                .Apply(r => state.Console.Write(SnapshotTableHelper.CreateIncomeTable(r)))
                .Apply(r => state.Console.Write(SnapshotTableHelper.CreateExpenseTable(r)));

    private static void PerformReconciliation(ReconciledSnapshot snapshot, IAnsiConsole console) =>
        console.Confirm(Constants.Reconcile.PerformReconcileLabel).IfTrueOrElse(
             () => console.WriteMessage("Deleting old entries... then saving month's consolidated data."),
             () => console.WriteMessage(Constants.Reconcile.CancelledMessage));
}
