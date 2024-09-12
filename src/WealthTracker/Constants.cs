namespace WealthTracker;

internal static class Constants
{
    public const string AppTitle = "Wealth Tracker";
    public const string AskCommandLabel = "Enter app command (show or exit):";
    public const string ExitCommandMessage = "[green]Good-bye![/]";
    public static readonly string[] CommandListMessage = 
    [
        " - list (l): shows list of the user's accounts.",
        " - add: allows user to input account data.",
        " - edit: allows uer to edit basic account data (name and categories).",
        " - delete (del): allows user to delete an existing account.",
        " - record (r): record the value for an account on a specified day.",
        " - unrecord (u): removed a recorded value from an account on a specified day.",
        " - current (c): list the current net worth based on latest account entries.",
        " - monthly (m): list monthly net worth for last 6 months.",
        " - show-commands (show): show list of the available commands in this app.",
        " - exit (x): leave the app.",
    ];

    public static string ErrorCommandMessage(string command) => 
        $"[red]Error:[/] The '{command}' conversion is unknown. Please select again...";
}
