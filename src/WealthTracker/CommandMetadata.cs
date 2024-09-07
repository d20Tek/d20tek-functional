namespace WealthTracker;

internal sealed class CommandMetadata
{
    public static Func<CommandTypeMetadata[]> GetCommandTypes = () =>
    [
        new ("show", ["show", "show-types"], CommandHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommandHandlers.Exit)
    ];

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommandHandlers.Error(x, command, t));
}
