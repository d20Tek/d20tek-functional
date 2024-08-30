namespace TreasureHunt;

internal static class Constants
{
    public const string GameTitle = "Treasure Hunt";
    public const string ShowInstructionsLabel = "WOULD YOU LIKE THE GAME INSTRUCTIONS?";
    public const string StartGameLabel = "PRESS ANY KEY TO START...";
    public const int TotalRooms = 7;
    public const int MovesAllowed = 28;

    public const string CommandInputLabel = "WHAT DO YOU WANT TO DO?";
    public const string EndGameLostMessage = "SORRY! YOU'VE RUN OUT OF MOVES.";
    public const string EndGameWonMessage = "CONGRATS! YOU WON THE GAME.";

    public static string[] CommandOptions = ["HELP", "N", "E", "S", "W", "GRAB", "PUT", "LOCATE",
                                             "help", "n", "e", "s", "w", "grab", "put", "locate"];
    public static readonly string[] Instructions =
    [
        "",
        "------------------------------------------------------",
        "THERE ARE SEVEN ROOMS IN THE MAZE, AND",
        "THERE IS A BOX OF IN EACH ONE. YOU MUST",
        "GET ALL THE BOXES INTO THE SAME ROOM.",
        "",
        "THESE ARE THE COMMANDS THE COMPUTER UNDERSTANDS:",
        "[yellow]HELP[/]    : TELLS YOU HOW TO PLAY",
        "[yellow]N,E,S,W[/] : MOVE NORTH, EAST, SOUTH, OR WEST",
        "[yellow]GRAB[/]    : PICK UP TREASURE IN THE ROOM",
        "[yellow]PUT[/]     : PUT DOWN TREASURE IN THE ROOM",
        "[yellow]LOCATE[/]  : PRINT CURRENT LOCATION OF TREASURE",
        "------------------------------------------------------",
        ""
    ];

    public static string[] RoomDescription(int room, string description) =>
    [
            "------------------------------------------------------",
            $"YOU ARE IN ROOM {room}",
            $"IT IS {description}",
    ];

    public static string[] TreasureDescriptions(Location[] locations) =>
        locations.Select(x => $"IT CONTAINS {GameData.GetTreasureById(x.TreasureId).Name}")
                 .ToArray();

    public static class Commands
    {
        public static string[] CannotMoveMessage = [string.Empty, "[yellow]YOU CANNOT MOVE THAT DIRECTION.[/]"];
        public static string[] OkMessage = [string.Empty, "OK."];
        public static string[] CommandError = [string.Empty, "[red]ERROR RUNNING COMMAND.[/]"];
    }
}
