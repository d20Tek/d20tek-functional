using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Expenses;

public partial class DeleteExpense
{
    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<Expense> _expense = Optional<Expense>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(e => e.Id, Id)
             .HandleResultAsync(s => _expense = s, e => _errorMessage = e);

    private async Task DeleteHandler() =>
        await _repo.GetByIdAsync(e => e.Id, Id)
             .BindAsync(exp => _repo.RemoveAsync(exp))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
