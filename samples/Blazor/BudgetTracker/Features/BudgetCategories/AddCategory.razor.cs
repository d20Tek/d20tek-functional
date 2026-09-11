using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;

namespace BudgetTracker.Features.BudgetCategories;

public partial class AddCategory
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public decimal BudgetedAmount { get; set; }
    }

    private Optional<string> _errorMessage = Optional<string>.None();
    private readonly ViewModel _vm = new();

    private async Task CreateCategory() =>
        await _repo.AddAsync(new BudgetCategory(Guid.NewGuid().GetHashCode(), _vm.Name, _vm.BudgetedAmount))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
