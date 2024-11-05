using BudgetTracker.Common;
using BudgetTracker.Domain;

namespace BudgetTracker.Features.Incomes;

public partial class AddIncome
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public DateTimeOffset DepositDate { get; set; } = DateTimeOffset.Now;

        public decimal Amount { get; set; }
    }

    private string _errorMessage = string.Empty;
    private readonly ViewModel _vm = new();

    private void CreateCategory() =>
        _repo.Create(new Income(0, _vm.Name, _vm.DepositDate, _vm.Amount))
             .HandleResult(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
