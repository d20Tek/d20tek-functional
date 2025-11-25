using D20Tek.Functional;
using Games.Common;
using Spectre.Console;
using Teletype;

AnsiConsole.Console.ToIdentity()
           .Iter(c => c.Write(Presenters.GameHeader(Constants.GameTitle)))
           .Iter(c => c.TeletypeMarkup(Constants.Introduction, Constants.TeletypeConfig))
           .Iter(c => c.WriteLine());
