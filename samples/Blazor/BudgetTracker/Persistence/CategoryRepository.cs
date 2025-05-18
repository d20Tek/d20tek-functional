using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class CategoryRepository : LowDbRepository<BudgetCategory, BudgetDbDocument>, ICategoryRepository
{
    public CategoryRepository(LowDb<BudgetDbDocument> db)
        : base(db, c => c.Categories.Entities)
    {
    }
}
