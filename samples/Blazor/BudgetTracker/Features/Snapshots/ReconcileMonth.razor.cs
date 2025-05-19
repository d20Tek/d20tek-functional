using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;

namespace BudgetTracker.Features.Snapshots;

public partial class ReconcileMonth
{
    internal class ViewModel
    {
        public DateTimeOffset ReconcileDate { get; set; } = DateTimeOffset.Now;

        public Option<ReconciledSnapshot> Snapshot { get; set; } = Option<ReconciledSnapshot>.None();
    }

    private Option<string> _errorMessage = Option<string>.None();
    private readonly ViewModel _vm = new();

    private void BuildSnapshotHandler() =>
        _vm.Snapshot = ReconciledBuilder.GenerateSnapshot(
            GetDateRange(_vm.ReconcileDate),
            _incRepo,
            _catRepo,
            _expRepo);

    private static DateRange GetDateRange(DateTimeOffset date) =>
        date.StartOfMonth()
            .Pipe(start => new DateRange(start, start.AddMonths(1)));

    private void ReconcileHandler() => SaveReconciledSnapshot(_vm.Snapshot.Get());

    private void SaveReconciledSnapshot(ReconciledSnapshot snapshot) =>
        Validate(snapshot, _snapRepo)
            .Bind(s => _snapRepo.Add(snapshot))
            .Bind(_ => _incRepo.RemoveByDateRange(snapshot.GetDateRange()))
            .Bind(_ => _expRepo.RemoveByDateRange(snapshot.GetDateRange()))
            .Iter(_ => SaveAllChanges())
            .HandleResult(s => _errorMessage = Constants.Reconcile.ReconcileSucceeded, e => _errorMessage = e);

    private void SaveAllChanges()
    {
        _snapRepo.SaveChanges();
        _incRepo.SaveChanges();
        _expRepo.SaveChanges();
    }

    private static Result<ReconciledSnapshot> Validate(
        ReconciledSnapshot snapshot,
        IReconciledSnapshotRepository repo) =>
        snapshot switch
        {
            { TotalIncome.Amount: <= 0, TotalExpenses.Actual: <= 0 } => 
                Constants.Reconcile.SnapshotEmptyError,
            _ when repo.Exists(s => s.StartDate == snapshot.StartDate).GetValue() =>
                Constants.Reconcile.SnapshotAlreadyExistsError,
            _ => snapshot
        };
}
