using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Expenses;

public partial class DeleteExpense
{
    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<Expense> _expense = Optional<Expense>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(e => e.Id, Id)
             .HandleResult(s => _expense = s, e => _errorMessage = e);

    private void DeleteHandler() =>
        _repo.GetById(e => e.Id, Id)
             .Map(exp => _repo.Remove(exp))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
