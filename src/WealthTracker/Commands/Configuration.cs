namespace WealthTracker.Commands;

internal sealed class Configuration
{
    public static CommandTypeMetadata[] GetCommandTypes() =>
    [
        new ("show", ["show", "show-commands"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit),
        new ("list", ["list", "l"], ListWealthEntriesCommand.Handle),
        new ("add", ["add", "a"], AddWealthEntryCommand.Handle),
        new ("delete", ["delete", "del", "d"], DeleteWealthEntryCommand.Handle),
        new ("edit", ["edit", "e"], EditWealthEntryCommand.Handle),
        new ("record", ["record", "r"], RecordAmountCommand.Handle),
        new ("record-date", ["record-date", "rd"], RecordPastAmountCommand.Handle),
        new ("unrecord", ["unrecord", "u"], UnrecordAmountCommand.Handle),
        new ("unrecord-date", ["unrecord-date", "ud"], RecordPastAmountCommand.Handle),
        new ("current", ["current", "c"], CurrentNetWorthCommand.Handle),
        new ("monthly", ["monthly", "m"], MonthlyNetWorthCommand.Handle),
    ];

    public static string[] GetCommands() =>
        GetCommandTypes().SelectMany(x => x.AllowedCommands).ToArray();

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
