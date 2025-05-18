using BudgetTracker.Domain;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Common;

internal interface ICategoryRepository : IRepository<BudgetCategory>;

//internal interface IExpenseRepository : IRepository<Expense>;

//internal interface IIncomeRepository : IRepository<Income>;

//internal interface IReconciledSnapshotRepository : IRepository<ReconciledSnapshot>;
