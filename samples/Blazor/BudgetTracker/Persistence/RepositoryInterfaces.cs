// define repository interface types here to be used throughout the project.

global using ICategoryRepository = Apps.Repositories.IRepository<BudgetTracker.Domain.BudgetCategory>;

global using IExpenseRepository = Apps.Repositories.IRepository<BudgetTracker.Domain.Expense>;

global using IIncomeRepository = Apps.Repositories.IRepository<BudgetTracker.Domain.Income>;

global using IReconciledSnapshotRepository = Apps.Repositories.IRepository<BudgetTracker.Domain.ReconciledSnapshot>;
