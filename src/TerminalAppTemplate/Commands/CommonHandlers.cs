using D20Tek.Functional;
using Spectre.Console;

namespace TerminalAppTemplate.Commands;

internal static class CommonHandlers
{
    public static AppState ShowCommands(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteLine(string.Join(Environment.NewLine, Constants.CommandListMessage)))
             .Map(s => s with { Command = metadata.Name });

    public static AppState Exit(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.MarkupLine(Constants.ExitCommandMessage))
             .Map(s => s with { Command = metadata.Name, CanContinue = false });

    public static AppState Error(AppState state, string command, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.MarkupLine(Constants.ErrorCommandMessage(command)))
             .Map(s => s with { Command = metadata.Name });
}
