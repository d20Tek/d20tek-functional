namespace TerminalAppTemplate.Commands;

internal sealed class Configuration
{
    public static Func<CommandTypeMetadata[]> GetCommandTypes = () =>
    [
        new ("help", ["help", "h"], CommonHandlers.ShowCommands),
        new ("exit", ["exit", "x"], CommonHandlers.Exit)
    ];

    public static string[] GetCommands() =>
        GetCommandTypes().SelectMany(x => x.AllowedCommands).ToArray();

    public static CommandTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommonHandlers.Error(x, command, t));
}
