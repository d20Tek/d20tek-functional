namespace WealthTracker.Commands;

internal sealed class Configuration
{
    public static Func<CommandTypeMetadata[]> GetCommandTypes = () =>
    [
        new ("show", ["show", "show-commands"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit)
    ];

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
