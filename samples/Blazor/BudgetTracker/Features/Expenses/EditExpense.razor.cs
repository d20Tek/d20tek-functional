using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
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

    protected override async Task OnInitializedAsync()
    {
        var categories = await _catRepo.GetAllAsync().MatchAsync(s => Task.FromResult(s.ToArray()), _ => Task.FromResult(Array.Empty<BudgetCategory>()));
        await _repo.GetByIdAsync(e => e.Id, Id)
             .HandleResultAsync(
                s => _vm = new ViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId,
                    Categories = categories,
                    CommittedDate = s.CommittedDate,
                    Actual = s.Actual
                },
                e => _errorMessage = e);
    }

    private async Task UpdateHandler() =>
        await _vm.MatchActionAsync(
            a => _repo.GetByIdAsync(e => e.Id, Id)
                      .MapAsync(prev => Task.FromResult(prev.UpdateExpense(a.Name, a.CategoryId, a.CommittedDate, a.Actual)))
                      .BindAsync(updated => _repo.UpdateAsync(updated))
                      .IterAsync(_ => _repo.SaveChangesAsync())
                      .HandleResultAsync(s => _nav.NavigateTo(Constants.Expense.ListUrl), e => _errorMessage = e),
            () => _errorMessage = Constants.Income.MissingIncomeError);

    private void CancelHandler() => _nav.NavigateTo(Constants.Expense.ListUrl);
}
