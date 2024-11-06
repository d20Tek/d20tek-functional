using BudgetTracker.Domain;

namespace BudgetTracker.Features.Expenses;

public partial class ListExpenses
{
    Expense[] _expenses = [];
    BudgetCategory[] _categories = [];

    protected override void OnInitialized()
    {
        _expenses = _repo.GetEntities();
        _categories = _catRepo.GetEntities();
    }

    private string CatIdToCategory(int catId) =>
        _categories.Any(x => x.Id == catId) ? _categories[catId].Name : "Missing category";
}
