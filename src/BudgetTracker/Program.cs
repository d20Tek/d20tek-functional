using BudgetTracker;
using BudgetTracker.Persistence;
using Spectre.Console;

var catRepo = RepositoryFactory.CreateCategoryRepository();
var expRepo = RepositoryFactory.CreateExpenseRepository();
var incomeRepo = RepositoryFactory.CreateIncomeRepository();

App.Run(AnsiConsole.Console, catRepo, expRepo, incomeRepo);