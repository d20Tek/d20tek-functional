using Apps.Common;
using BudgetTracker.Entities;
using BudgetTracker.Persistence;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ReconcileMonthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Reconcile.HeaderLabel))
             .Map(s => state.CreateReconciledSnapshot(s.Console.GetMonth()))
             .Iter(r => state.PerformReconciliation(r))
             .Map(r => state with { Command = metadata.Name });

    private static DateRange GetMonth(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Reconcile.DateLabel, DateTimeOffset.Now.StartOfMonth())
            .StartOfMonth()
            .Pipe(start => new DateRange(start, start.AddMonths(1)));

    private static ReconciledSnapshot CreateReconciledSnapshot(this AppState state, DateRange range) =>
        ReconciledBuilder.GenerateSnapshot(range, state.IncomeRepo, state.CategoryRepo, state.ExpenseRepo).ToIdentity()
            .Iter(r => state.Console.WriteMessage(Constants.Reconcile.ItemsToReconcile(range.Start.ToMonth())))
            .Iter(r => state.Console.Write(SnapshotTableHelper.CreateIncomeTable(r)))
            .Iter(r => state.Console.Write(SnapshotTableHelper.CreateExpenseTable(r)));

    private static void PerformReconciliation(this AppState state, ReconciledSnapshot snapshot) =>
        state.Console.Confirm(Constants.Reconcile.PerformReconcileLabel).IfTrueOrElse(
             () => SaveReconciledSnapshot(snapshot, state),
             () => state.Console.WriteMessage(Constants.Reconcile.CancelledMessage));

    private static void SaveReconciledSnapshot(ReconciledSnapshot snapshot, AppState state) =>
        snapshot.Validate(state.SnapshotRepo)
            .Map(s => state.SnapshotRepo.Create(snapshot))
            .Map(_ => state.IncomeRepo.DeleteByDateRange(snapshot.GetDateRange()))
            .Map(_ => state.ExpenseRepo.DeleteByDateRange(snapshot.GetDateRange()))
            .Iter(result => state.Console.DisplayResult(result, s => Constants.Reconcile.ReconcileSucceeded));

    private static Result<ReconciledSnapshot> Validate(
        this ReconciledSnapshot snapshot,
        IReconciledSnapshotRepository repo) =>
        snapshot switch
        {
            { TotalIncome.Amount: <= 0, TotalExpenses.Actual: <= 0 } => Constants.Reconcile.SnapshotEmptyError,
            _ when repo.SnapshotExists(snapshot.StartDate) => Constants.Reconcile.SnapshotAlreadyExistsError,
            _ => snapshot
        };
}
