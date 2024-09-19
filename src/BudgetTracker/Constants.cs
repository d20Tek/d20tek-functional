namespace BudgetTracker;

internal static class Constants
{
    public const string AppTitle = "Budget Tracker";
    public const string AskCommandLabel = "Enter app command (help or exit):";
    public const string ExitCommandMessage = "[green]Good-bye![/]";
    public static readonly string[] CommandListMessage =
    [
        " - help (h): show list of the available commands in this app.",
        " - exit (x): leave the app.",
    ];

    public static string ErrorCommandMessage(string command) =>
        $"[red]Error:[/] The '{command}' conversion is unknown. Please select again...";
}
