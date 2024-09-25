using BudgetTracker;
using BudgetTracker.Persistence;
using Spectre.Console;

App.Run(AppState.Init(
    AnsiConsole.Console,
    RepositoryFactory.CreateCategoryRepository(),
    RepositoryFactory.CreateExpenseRepository(),
    RepositoryFactory.CreateIncomeRepository(),
    RepositoryFactory.CreateSnapshotRepository()));