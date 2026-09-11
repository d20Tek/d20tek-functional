using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Incomes;

public partial class DeleteIncome
{
    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<Income> _income = Optional<Income>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(i => i.Id, Id)
             .HandleResultAsync(s => _income = s, e => _errorMessage = e);

    private async Task DeleteHandler() =>
        await _repo.GetByIdAsync(i => i.Id, Id)
             .BindAsync(income => _repo.RemoveAsync(income))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
