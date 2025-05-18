// define repository interface types here to be used throughout the project.

global using IExpenseRepository = Apps.Repositories.IRepository<BudgetTracker.Domain.Expense>;

global using IReconciledSnapshotRepository = Apps.Repositories.IRepository<BudgetTracker.Domain.ReconciledSnapshot>;
