using D20Tek.LowDb;
using Spectre.Console;
using WealthTracker;
using WealthTracker.Persistence;

var db = LowDbFactory.CreateLowDb<WealthDataStore>(b => b
    .UseFileDatabase("wealth-data.json")
    .WithFolder(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WealthTracker"));
var repo = new WealthFileRepository(db);
App.Run(AnsiConsole.Console, repo);