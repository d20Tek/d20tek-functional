using Apps.Common;
using BudgetTracker.Entities;
using BudgetTracker.Persistence;
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
            .Apply(r => state.PerformReconciliation(r))
            .Map(r => state with { Command = metadata.Name });

    private static (DateTimeOffset Start, DateTimeOffset End) GetMonth(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Reconcile.DateLabel, DateTimeOffset.Now.StartOfMonth())
            .StartOfMonth()
            .Map(start => (start, start.AddMonths(1)));

    private static ReconciledSnapshot CreateReconciledSnapshot(
        this AppState state,
        DateTimeOffset start,
        DateTimeOffset end) =>
        ReconciledBuilder.GenerateSnapshot(start, end, state.IncomeRepo, state.CategoryRepo, state.ExpenseRepo)
            .Apply(r => state.Console.WriteMessage(Constants.Reconcile.ItemsToReconcile(start.ToMonth())))
            .Apply(r => state.Console.Write(SnapshotTableHelper.CreateIncomeTable(r)))
            .Apply(r => state.Console.Write(SnapshotTableHelper.CreateExpenseTable(r)));

    private static void PerformReconciliation(this AppState state, ReconciledSnapshot snapshot) =>
        state.Console.Confirm(Constants.Reconcile.PerformReconcileLabel).IfTrueOrElse(
             () => SaveReconciledSnapshot(snapshot, state),
             () => state.Console.WriteMessage(Constants.Reconcile.CancelledMessage));

    private static void SaveReconciledSnapshot(ReconciledSnapshot snapshot, AppState state) =>
        snapshot.Validate(state.SnapshotRepo)
            .Bind(s => state.SnapshotRepo.Create(snapshot))
            .Bind(_ => state.IncomeRepo.DeleteByDateRange(snapshot.StartDate, snapshot.EndDate))
            .Bind(_ => state.ExpenseRepo.DeleteByDateRange(snapshot.StartDate, snapshot.EndDate))
            .Bind(_ => snapshot)
            .Apply(result => state.Console.DisplayMaybe(result, s => Constants.Reconcile.ReconcileSucceeded));

    private static Maybe<ReconciledSnapshot> Validate(
        this ReconciledSnapshot snapshot,
        IReconciledSnapshotRepository repo) =>
        snapshot switch
        {
            { TotalIncome.Amount: <= 0, TotalExpenses.Actual: <= 0 } => Constants.Reconcile.SnapshotEmptyError,
            _ when repo.SnapshotExists(snapshot.StartDate) => Constants.Reconcile.SnapshotAlreadyExistsError,
            _ => snapshot
        };
}
