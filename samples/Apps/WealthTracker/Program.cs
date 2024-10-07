using Spectre.Console;
using WealthTracker;
using WealthTracker.Persistence;

App.Run(AnsiConsole.Console, RepositoryFactory.CreateWealthRepository());