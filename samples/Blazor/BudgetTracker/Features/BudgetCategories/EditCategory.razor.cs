using BudgetTracker.Common;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.BudgetCategories;

public partial class EditCategory
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal BudgetedAmount { get; set; }
    }

    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<ViewModel> _vm = Optional<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(c => c.Id, Id)
             .HandleResultAsync(
                s => _vm = new ViewModel { Id = s.Id, Name = s.Name, BudgetedAmount = s.BudgetedAmount },
                e => _errorMessage = e);

    private async Task UpdateHandler() =>
        await _vm.MatchActionAsync(
            a => _repo.GetByIdAsync(c => c.Id, Id)
                      .MapAsync(prev => Task.FromResult(prev.UpdateCategory(a.Name, a.BudgetedAmount)))
                      .BindAsync(updated => _repo.UpdateAsync(updated))
                      .IterAsync(_ => _repo.SaveChangesAsync())
                      .HandleResultAsync(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Categories.MissingCategoryError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
