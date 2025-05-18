using BudgetTracker.Domain;

namespace BudgetTracker.Features.Expenses;

public partial class ListExpenses
{
    Expense[] _expenses = [];
    BudgetCategory[] _categories = [];

    protected override void OnInitialized()
    {
        _expenses = _repo.GetEntities();
        _categories = _catRepo.GetAll().Match(s => s.ToArray(), _ => []);
    }

    private string CatIdToCategory(int catId) =>
        _categories.FirstOrDefault(x => x.Id == catId)?.Name ?? "Missing category";
}
