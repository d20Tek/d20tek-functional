using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class CommonHandlers
{
    public static AppState ShowCommands(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteLine(string.Join(Environment.NewLine, ["list of available commands..."])))
             .Map(s => s with { Command = metadata.Name });

    public static AppState Exit(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.MarkupLine("[green]Good-bye![/]"))
             .Map(s => s with { Command = metadata.Name, CanContinue = false });

    public static AppState Error(AppState state, string command, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.MarkupLine($"[red]Error:[/] The '{command}' conversion is unknown. Please select again..."))
             .Map(s => s with { Command = metadata.Name });
}
