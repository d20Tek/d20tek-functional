using BudgetTracker;
using BudgetTracker.Persistence;
using Spectre.Console;

var catRepo = RepositoryFactory.CreateCategoryRepository();
var expRepo = RepositoryFactory.CreateExpenseRepository();
var incRepo = RepositoryFactory.CreateIncomeRepository();
var snapRepo = RepositoryFactory.CreateSnapshotRepository();

App.Run(AnsiConsole.Console, catRepo, expRepo, incRepo, snapRepo);