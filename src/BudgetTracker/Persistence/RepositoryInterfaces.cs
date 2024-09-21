// define repository interface types here to be used throughout the project.

global using ICategoryRepository = Apps.Repositories.IRepository<BudgetTracker.Entities.BudgetCategory>;

global using IExpenseRepository = Apps.Repositories.IRepository<BudgetTracker.Entities.Expense>;

global using IIncomeRepository = Apps.Repositories.IRepository<BudgetTracker.Entities.Income>;
