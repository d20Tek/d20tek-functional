using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;

namespace BudgetTracker.Features.Snapshots;

public partial class ReconcileMonth
{
    internal class ViewModel
    {
        public DateTimeOffset ReconcileDate { get; set; } = DateTimeOffset.Now;

        public Optional<ReconciledSnapshot> Snapshot { get; set; } = Optional<ReconciledSnapshot>.None();
    }

    private Optional<string> _errorMessage = Optional<string>.None();
    private readonly ViewModel _vm = new();

    private async Task BuildSnapshotHandler() =>
        _vm.Snapshot = await ReconciledBuilder.GenerateSnapshot(
            GetDateRange(_vm.ReconcileDate),
            _incRepo,
            _catRepo,
            _expRepo);

    private static DateRange GetDateRange(DateTimeOffset date) =>
        date.StartOfMonth()
            .Pipe(start => new DateRange(start, start.AddMonths(1)));

    private Task ReconcileHandler() => SaveReconciledSnapshot(_vm.Snapshot.Get());

    private async Task SaveReconciledSnapshot(ReconciledSnapshot snapshot) =>
        await (await Validate(snapshot, _snapRepo))
            .BindAsync(s => _snapRepo.AddAsync(snapshot))
            .BindAsync(_ => _incRepo.RemoveByDateRange(snapshot.GetDateRange()))
            .BindAsync(_ => _expRepo.RemoveByDateRange(snapshot.GetDateRange()))
            .IterAsync(_ => SaveAllChanges())
            .HandleResultAsync(s => _errorMessage = Constants.Reconcile.ReconcileSucceeded, e => _errorMessage = e);

    private async Task SaveAllChanges()
    {
        await _snapRepo.SaveChangesAsync();
        await _incRepo.SaveChangesAsync();
        await _expRepo.SaveChangesAsync();
    }

    private static async Task<Result<ReconciledSnapshot>> Validate(
        ReconciledSnapshot snapshot,
        IReconciledSnapshotRepository repo) =>
        snapshot switch
        {
            { TotalIncome.Amount: <= 0, TotalExpenses.Actual: <= 0 } => 
                Constants.Reconcile.SnapshotEmptyError,
            _ when (await repo.ExistsAsync(s => s.StartDate == snapshot.StartDate)).GetValue() =>
                Constants.Reconcile.SnapshotAlreadyExistsError,
            _ => snapshot
        };
}
