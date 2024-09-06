using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;
using Teletype;

AnsiConsole.Console
    .Apply(c => c.Write(Presenters.GameHeader(Constants.GameTitle)))
    .Apply(c => c.TeletypeMarkup(Constants.Introduction, Constants.TeletypeConfig))
    .Apply(c => c.WriteLine());

