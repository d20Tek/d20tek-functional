using Spectre.Console;

namespace WealthTracker;

internal static class CommandHandlers
{
    public static AppState ShowCommands(AppState prevState, CommandTypeMetadata metadata)
    {
        var newState = prevState with { Command = metadata.Name };

        newState.Console.WriteLine(
            string.Join(Environment.NewLine, ["list of available commands..."]));

        return newState;
    }

    public static AppState Exit(AppState prevState, CommandTypeMetadata metadata)
    {
        prevState.Console.MarkupLine("[green]Good-bye![/]");
        return prevState with { Command = metadata.Name, CanContinue = false };
    }

    public static AppState Error(AppState prevState, string command, CommandTypeMetadata metadata)
    {
        prevState.Console.MarkupLine($"[red]Error:[/] The '{command}' conversion is unknown. Please select again...");
        return prevState with { Command = metadata.Name };
    }
}
