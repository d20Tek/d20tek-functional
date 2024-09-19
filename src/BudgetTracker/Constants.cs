namespace BudgetTracker;

internal static class Constants
{
    public const string AppTitle = "Budget Tracker";
    public const string AskCommandLabel = "Enter app command (help or exit):";
    public const string ExitCommandMessage = "[green]Good-bye![/]";
    public static readonly string[] CommandListMessage =
    [
        string.Empty,
        "[green]--- Command Definitions[/] ---",
        string.Empty,
        "[yellow]Budget Categories:[/]",
        " - list-cat (lc): shows list of the user's budget categories.",
        " - add-cat (ac): allows user to input new budget category.",
        " - edit-cat (ec): allows uer to edit basic category data (name and amount).",
        " - delete-cat (dc): allows user to delete an existing budget category.",
        string.Empty,
        "[yellow]Common Commands:[/]",
        " - help (h): show list of the available commands in this app.",
        " - exit (x): leave the app.",
    ];

    public static string ErrorCommandMessage(string command) =>
        $"[red]Error:[/] The '{command}' conversion is unknown. Please select again...";
}
