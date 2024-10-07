namespace WealthTracker;

internal static class Constants
{
    public const string AppTitle = "Wealth Tracker";
    public const string AskCommandLabel = "Enter app command (help or exit):";
    public const string ExitCommandMessage = "[green]Good-bye![/]";
    public static readonly string[] CommandListMessage = 
    [
        " - list (l): shows list of the user's accounts.",
        " - add: allows user to input account data.",
        " - edit: allows uer to edit basic account data (name and categories).",
        " - delete (del): allows user to delete an existing account.",
        " - record (r): record the value for today on an account.",
        " - unrecord (u): remove today's recorded value from an account.",
        " - current (c): list the current net worth based on latest account entries.",
        " - monthly (m): list monthly net worth for last 6 months.",
        " - yearly (y): list yearly net worth for last 5 years.",
        " - record-date (rd): record the value on an account for specified date in the past.",
        " - unrecord-date (ud): remove recorded value from an account on specified date in the past.",
        " - help (h): show list of the available commands in this app.",
        " - exit (x): leave the app.",
    ];

    public static string ErrorCommandMessage(string command) => 
        $"[red]Error:[/] The '{command}' conversion is unknown. Please select again...";

    public static Exception FutureDateError(string propertyName) =>
        new ArgumentOutOfRangeException(propertyName, "Date value for updates cannot be in the future.");
}
