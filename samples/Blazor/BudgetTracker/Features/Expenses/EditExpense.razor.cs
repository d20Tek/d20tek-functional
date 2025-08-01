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

    private Optional<string> _errorMessage = Optional<string>.None();
    private Optional<ViewModel> _vm = Optional<ViewModel>.None();

    [Parameter]
    public int Id { get; set; }

    protected override void OnInitialized() =>
        _repo.GetById(e => e.Id, Id)
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
            a => _repo.GetById(e => e.Id, Id)
                      .Map(prev => prev.UpdateExpense(a.Name, a.CategoryId, a.CommittedDate, a.Actual))
                      .Map(updated => _repo.Update(updated))
                      .Iter(_ => _repo.SaveChanges())
                      .HandleResult(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Income.MissingIncomeError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
