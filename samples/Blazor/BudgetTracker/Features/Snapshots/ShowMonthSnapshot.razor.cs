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

    protected override async Task OnInitializedAsync() =>
        await _snapRepo.GetByIdAsync(s => s.Id, Id)
                       .HandleResultAsync(s => _snapshot = s, e => _errorMessage = e);
}
