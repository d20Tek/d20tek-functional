using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;

namespace BudgetTracker.Features.Incomes;

public partial class AddIncome
{
    public class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public DateTimeOffset DepositDate { get; set; } = DateTimeOffset.Now;

        public decimal Amount { get; set; }
    }

    private Optional<string> _errorMessage = Optional<string>.None();
    private readonly ViewModel _vm = new();

    private async Task CreateHandler() =>
        await _repo.AddAsync(new Income(Guid.NewGuid().GetHashCode(), _vm.Name, _vm.DepositDate, _vm.Amount))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
