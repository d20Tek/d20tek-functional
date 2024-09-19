using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal interface ICategoryRepository
{
    Maybe<BudgetCategory> Create(BudgetCategory category);

    Maybe<BudgetCategory> Delete(int id);

    BudgetCategory[] GetCategories();

    Maybe<BudgetCategory> GetCategoryById(int id);

    Maybe<BudgetCategory> Update(BudgetCategory category);
}
