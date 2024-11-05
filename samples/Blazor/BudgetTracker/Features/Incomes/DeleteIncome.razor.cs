using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Incomes;

public partial class DeleteIncome
{
    private string _errorMessage = string.Empty;
    private Option<Income> _income = Option<Income>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetEntityById(Id)
             .HandleResult(s => _income = s, e => _errorMessage = e);

    private void DeleteHandler() =>
        _repo.Delete(Id)
             .HandleResult(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
