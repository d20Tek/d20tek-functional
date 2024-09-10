namespace TerminalAppTemplate.Commands;

internal sealed record CommandTypeMetadata(
    string Name,
    string[] AllowedCommands,
    Func<AppState, CommandTypeMetadata, AppState> TypeHandler);
