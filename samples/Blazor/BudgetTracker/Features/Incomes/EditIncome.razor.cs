using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Incomes;

public partial class EditIncome
{
    public class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTimeOffset DepositDate { get; set; }

        public decimal Amount { get; set; }
    }

    private Option<string> _errorMessage = Option<string>.None();
    private Option<ViewModel> _vm = Option<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetEntityById(Id)
             .HandleResult(
                s => _vm = new ViewModel { Id = s.Id, Name = s.Name, DepositDate = s.DepositDate, Amount = s.Amount },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _vm.MatchAction(
            a => _repo.Update(new Income(a.Id, a.Name, a.DepositDate, a.Amount))
                      .HandleResult(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Income.MissingIncomeError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
