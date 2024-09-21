using BudgetTracker;
using BudgetTracker.Persistence;
using Spectre.Console;

var catRepo = RepositoryFactory.CreateCategoryRepository();
var expRepo = RepositoryFactory.CreateExpenseRepository();
var credRepo = RepositoryFactory.CreateCreditRepository();

App.Run(AnsiConsole.Console, catRepo, expRepo, credRepo);