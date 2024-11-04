using BudgetTracker.Common;
using BudgetTracker.Domain;

namespace BudgetTracker.Features.BudgetCategories;

public partial class AddCategory
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public decimal BudgetedAmount { get; set; }
    }

    private string _errorMessage = string.Empty;
    private readonly ViewModel _vm = new();

    private void CreateCategory() =>
        _repo.Create(new BudgetCategory(0, _vm.Name, _vm.BudgetedAmount))
             .HandleResult(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
