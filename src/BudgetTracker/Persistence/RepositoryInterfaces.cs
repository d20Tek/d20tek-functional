﻿// define repository interface types here to be used throughout the project.

global using ICategoryRepository = Apps.Repositories.IRepositoryOld<BudgetTracker.Entities.BudgetCategory>;

global using IExpenseRepository = Apps.Repositories.IRepositoryOld<BudgetTracker.Entities.Expense>;

global using IIncomeRepository = Apps.Repositories.IRepositoryOld<BudgetTracker.Entities.Income>;

global using IReconciledSnapshotRepository = Apps.Repositories.IRepositoryOld<BudgetTracker.Entities.ReconciledSnapshot>;
