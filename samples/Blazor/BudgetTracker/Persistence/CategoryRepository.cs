using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class CategoryRepository(LowDb<BudgetDbDocument> db) : 
    LowDbRepository<BudgetCategory, BudgetDbDocument>(db, c => c.Categories.Entities), ICategoryRepository
{
}
