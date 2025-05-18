using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using Microsoft.AspNetCore.Components;

namespace BudgetTracker.Features.Expenses;

public partial class EditExpense
{
    internal class ViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public BudgetCategory[] Categories { get; set; } = [];

        public DateTimeOffset CommittedDate { get; set; } = DateTimeOffset.Now;

        public decimal Actual { get; set; }
    }

    private Option<string> _errorMessage = Option<string>.None();
    private Option<ViewModel> _vm = Option<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetEntityById(Id)
             .HandleResult(
                s => _vm = new ViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId,
                    Categories = _catRepo.GetAll().GetValue().ToArray(),
                    CommittedDate = s.CommittedDate,
                    Actual = s.Actual
                },
                e => _errorMessage = e);

    private void UpdateHandler() =>
        _vm.MatchAction(
            a => _repo.Update(new Expense(a.Id, a.Name, a.CategoryId, a.CommittedDate, a.Actual))
                      .HandleResult(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Income.MissingIncomeError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
