namespace WealthTracker.Commands;

internal sealed class Configuration
{
    public static CommandTypeMetadata[] GetCommandTypes() =>
    [
        new ("show", ["show", "show-commands"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit),
        new ("list", ["list", "l"], ListWealthEntriesCommand.Handle),
        new ("add", ["add"], AddWealthEntryCommand.Handle),
        new ("delete", ["delete", "del"], DeleteWealthEntryCommand.Handle)
    ];

    public static string[] GetCommands() =>
        GetCommandTypes().SelectMany(x => x.AllowedCommands).ToArray();

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
