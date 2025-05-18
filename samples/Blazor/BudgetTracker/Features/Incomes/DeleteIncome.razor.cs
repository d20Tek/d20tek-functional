using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Incomes;

public partial class DeleteIncome
{
    private Option<string> _errorMessage = Option<string>.None();
    private Option<Income> _income = Option<Income>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(i => i.Id, Id)
             .HandleResult(s => _income = s, e => _errorMessage = e);

    private void DeleteHandler() =>
        _repo.GetById(i => i.Id, Id)
             .Bind(income => _repo.Remove(income))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
