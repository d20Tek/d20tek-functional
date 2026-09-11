using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.BudgetCategories;

public partial class DeleteCategory
{
    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<BudgetCategory> _category = Optional<BudgetCategory>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(c => c.Id, Id)
             .HandleResultAsync(s => _category = s, e => _errorMessage = e);

    private async Task DeleteHandler() =>
        await _repo.GetByIdAsync(c => c.Id, Id)
             .BindAsync(cat => _repo.RemoveAsync(cat))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
