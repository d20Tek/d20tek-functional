using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Snapshots;

public partial class ShowMonthSnapshot
{
    private Option<ReconciledSnapshot> _snapshot = Option<ReconciledSnapshot>.None();
    private Option<string> _errorMessage = Option<string>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _snapRepo.GetEntityById(Id)
                 .HandleResult(s => _snapshot = s, e => _errorMessage = e);
}
