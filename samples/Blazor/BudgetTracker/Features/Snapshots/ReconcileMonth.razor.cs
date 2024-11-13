using BudgetTracker.Domain;

namespace BudgetTracker.Features.Snapshots;

public partial class ReconcileMonth
{
    internal class ViewModel
    {
        public DateTimeOffset ReconcileDate { get; set; } = DateTimeOffset.Now;
    }

    private string _errorMessage = string.Empty;
    private readonly ViewModel _vm = new();

    private void ReconcileHandler() { }
}
