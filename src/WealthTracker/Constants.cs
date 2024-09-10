namespace WealthTracker;

internal static class Constants
{
    public const string AppTitle = "Terminal App";
    public const string AskCommandLabel = "Enter app command (show or exit):";
    public const string ExitCommandMessage = "[green]Good-bye![/]";
    public static readonly string[] CommandListMessage = 
    [
        " - show-commands (show): show list of the available commands in this app.",
        " - exit (x): leave the app.",
    ];

    public static string ErrorCommandMessage(string command) => 
        $"[red]Error:[/] The '{command}' conversion is unknown. Please select again...";
}
