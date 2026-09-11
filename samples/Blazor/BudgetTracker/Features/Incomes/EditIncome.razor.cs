using BudgetTracker.Common;
using D20Tek.Functional;
using D20Tek.Functional.Async;
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

    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<ViewModel> _vm = Optional<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync() =>
        await _repo.GetByIdAsync(i => i.Id, Id)
             .HandleResultAsync(
                s => _vm = new ViewModel { Id = s.Id, Name = s.Name, DepositDate = s.DepositDate, Amount = s.Amount },
                e => _errorMessage = e);

    private async Task UpdateHandler() =>
        await _vm.MatchActionAsync(
            a => _repo.GetByIdAsync(i => i.Id, Id)
                      .MapAsync(prev => Task.FromResult(prev.UpdateIncome(a.Name, a.DepositDate, a.Amount)))
                      .BindAsync(updated => _repo.UpdateAsync(updated))
                      .IterAsync(_ => _repo.SaveChangesAsync())
                      .HandleResultAsync(s => _nav.NavigateTo(Constants.Income.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Income.MissingIncomeError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Income.ListUrl);
}
