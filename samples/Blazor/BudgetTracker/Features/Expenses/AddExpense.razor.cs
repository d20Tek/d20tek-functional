using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;

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

    private Optional<string> _errorMessage = Optional<string>.None();
    private readonly ViewModel _vm = new();

    protected override async Task OnInitializedAsync() =>
        _vm.Categories = await _catRepo.GetAllAsync()
                                       .MatchAsync(
                                            s => Task.FromResult(s.ToArray()),
                                            _ => Task.FromResult(Array.Empty<BudgetCategory>()));

    private async Task CreateHandler() =>
        await _repo.AddAsync(new Expense(Guid.NewGuid().GetHashCode(), _vm.Name, _vm.CategoryId, _vm.CommittedDate, _vm.Actual))
             .IterAsync(_ => _repo.SaveChangesAsync())
             .HandleResultAsync(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
