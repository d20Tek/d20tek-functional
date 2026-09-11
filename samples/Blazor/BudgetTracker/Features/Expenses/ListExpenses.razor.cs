using BudgetTracker.Domain;
using D20Tek.Functional.Async;

namespace BudgetTracker.Features.Expenses;

public partial class ListExpenses
{
    Expense[] _expenses = [];
    BudgetCategory[] _categories = [];

    protected override async Task OnInitializedAsync()
    {
        _expenses = await _repo.GetAllAsync()
                               .MatchAsync(
                                    s => Task.FromResult(s.ToArray()),
                                    _ => Task.FromResult(Array.Empty<Expense>()));

        _categories = await _catRepo.GetAllAsync()
                                    .MatchAsync(
                                        s => Task.FromResult(s.ToArray()),
                                        _ => Task.FromResult(Array.Empty<BudgetCategory>()));
    }

    private string CatIdToCategory(int catId) =>
        _categories.FirstOrDefault(x => x.Id == catId)?.Name ?? "Missing category";
}
