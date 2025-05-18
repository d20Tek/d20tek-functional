using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;

namespace BudgetTracker.Features.Incomes;

public partial class AddIncome
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public DateTimeOffset DepositDate { get; set; } = DateTimeOffset.Now;

        public decimal Amount { get; set; }
    }

    private Option<string> _errorMessage = Option<string>.None();
    private readonly ViewModel _vm = new();

    private void CreateHandler() =>
        _repo.Add(new Income(Guid.NewGuid().GetHashCode(), _vm.Name, _vm.DepositDate, _vm.Amount))
             .Iter(_ => _repo.SaveChanges())
             .HandleResult(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
