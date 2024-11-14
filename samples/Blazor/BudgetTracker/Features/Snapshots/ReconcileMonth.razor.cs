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

    private string _errorMessage = string.Empty;
    private readonly ViewModel _vm = new();

    private void BuildSnapshotHandler() { }
}
