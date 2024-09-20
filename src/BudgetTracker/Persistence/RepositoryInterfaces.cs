﻿// define repository interface types here to be used throughout the project.

global using ICategoryRepository = Apps.Repositories.IRepository<BudgetTracker.Entities.BudgetCategory>;

global using IExpenseRepository = Apps.Repositories.IRepository<BudgetTracker.Entities.Expense>;