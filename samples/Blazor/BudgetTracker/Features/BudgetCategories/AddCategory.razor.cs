using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;

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

    private void CreateCategory() =>
        _repo.Add(new BudgetCategory(Guid.NewGuid().GetHashCode(), _vm.Name, _vm.BudgetedAmount))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Categories.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Categories.ListUrl);
}
