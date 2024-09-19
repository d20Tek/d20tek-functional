using BudgetTracker;
using BudgetTracker.Persistence;
using Spectre.Console;

var catRepo = RepositoryFactory.CreateCategoryRepository();
App.Run(AnsiConsole.Console, catRepo);