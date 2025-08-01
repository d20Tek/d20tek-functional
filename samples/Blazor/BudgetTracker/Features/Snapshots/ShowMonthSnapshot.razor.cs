using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Snapshots;

public partial class ShowMonthSnapshot
{
    private Optional<ReconciledSnapshot> _snapshot = Optional<ReconciledSnapshot>.None();
    private Optional<string> _errorMessage = Optional<string>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _snapRepo.GetById(s => s.Id, Id)
                 .HandleResult(s => _snapshot = s, e => _errorMessage = e);
}
