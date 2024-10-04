using D20Tek.Functional;
using Spectre.Console;
using W = WealthTracker;

namespace WealthTracker.Commands;

internal static class CommonHandlers
{
    public static AppState ShowCommands(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteLine(string.Join(Environment.NewLine, W.Constants.CommandListMessage)))
             .Map(s => s with { Command = metadata.Name });

    public static AppState Exit(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.MarkupLine(W.Constants.ExitCommandMessage))
             .Map(s => s with { Command = metadata.Name, CanContinue = false });

    public static AppState Error(AppState state, string command, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.MarkupLine(W.Constants.ErrorCommandMessage(command)))
             .Map(s => s with { Command = metadata.Name });
}
