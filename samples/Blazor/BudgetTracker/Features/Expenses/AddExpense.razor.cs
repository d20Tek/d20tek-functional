using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;

namespace BudgetTracker.Features.Expenses;

public partial class AddExpense
{
    internal class ViewModel
    {
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public BudgetCategory[] Categories { get; set; } = [];

        public DateTimeOffset CommittedDate { get; set; } = DateTimeOffset.Now;

        public decimal Actual { get; set; }
    }

    private Option<string> _errorMessage = Option<string>.None();
    private readonly ViewModel _vm = new();

    protected override void OnInitialized() =>
        _vm.Categories = _catRepo.GetAll().Match(s => s.ToArray(), _ => []);

    private void CreateHandler() =>
        _repo.Create(new Expense(0, _vm.Name, _vm.CategoryId, _vm.CommittedDate, _vm.Actual))
             .HandleResult(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
