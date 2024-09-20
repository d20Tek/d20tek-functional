using BudgetTracker;
using BudgetTracker.Persistence;
using Spectre.Console;

var catRepo = RepositoryFactory.CreateCategoryRepository();
var expRepo = RepositoryFactory.CreateExpenseRepository();
App.Run(AnsiConsole.Console, catRepo, expRepo);