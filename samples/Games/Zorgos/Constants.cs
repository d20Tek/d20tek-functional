namespace Zorgos;

internal static class Constants
{
    public const string GameTitle = "Escape Zorgos";
    public const string ShowInstructionsLabel = "Would you like the game instructions?";
    public const string StartGameLabel = "Press any key to start...";
    public static string[] NextRoundLabel = ["", "<continue>..."];
    public static string[] WonGameMsg = ["", "[green]You WIN[/]!! Enjoy your flight back home..."];
    public static string[] LostGameMsg = 
        ["", "Sorry! You've [red]lost[/] all of your Zchips.", "You are stuck on Zorgos forever."];

    public static readonly string[] Instructions =
    [
        "",
        "------------------------------------------------------",
        "You are stranded on the alien planet Zorgos.",
        "And you need 50 Zchips to fill your ship with",
        "enough fuel to get back to Earth.",
        "",
        "You've been given 10 Zchips (the local currency),",
        "but the only way to get any more is to win them",
        "by playing a risky game of chance with an alien,",
        "super-intelligent computer.",
        "",
        "Place your bets... roll the dice...",
        "and see where you luck will take you.",
        "------------------------------------------------------",
        ""
    ];

    public static string TotalChipsLabel(int total) => $"You have {total} chips.";
    public const string PlaceBetLabel = "Place your bet";
    public const string RollDiceLabel = "Press any key to throw...";
    public const int BetDefaultValue = 5;
    public const int StartingChips = 10;
    public const int WinningTotal = 50;
    public const int TextLeftOffset = 10;
    public const int TextTopOffset = 5;

    public static class RollResult
    {
        public const string Push = "Keep your bet.";
        public const string Double = "You doubled your bet.";
        public static string Triple = "You tripled your bet.";
        public static string Lost = "[red]You lost your bet.[/]";
        public static string Total(int x) => $"You have {x} chips remaining.";
    }
}
