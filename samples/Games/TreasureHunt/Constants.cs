using TreasureHunt.Data;

namespace TreasureHunt;

internal static partial class Constants
{
    public const string GameTitle = "Treasure Hunt";
    public const string ShowInstructionsLabel = "WOULD YOU LIKE THE GAME INSTRUCTIONS?";
    public const string StartGameLabel = "PRESS ANY KEY TO START...";
    public const int TotalRooms = 7;
    public const int MovesAllowed = 29;
    public const int NoTreasure = 0;
    public const int DirectionNotAllowed = 0;
    public const int NoRoom = 0;

    public const string CommandInputLabel = "WHAT DO YOU WANT TO DO?";
    public const string EndGameLostMessage = "[red]SORRY![/] YOU'VE RUN OUT OF MOVES.";
    public static string[] EndGameWonMessage(int room, int moves) =>
    [
        "[green]WELL DONE![/] YOU GOT ALL OF THE TREASURE",
        $"INTO ROOM {room} IN {moves} MOVES."
    ];

    public static string[] CommandOptions = ["HELP", "N", "E", "S", "W", "GRAB", "PUT", "LOCATE",
                                             "help", "n", "e", "s", "w", "grab", "put", "locate"];
    public static readonly string[] Instructions =
    [
        "",
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
        ""
    ];

    public static string[] RoomDescription(int room) =>
    [
            "------------------------------------------------------",
            $"YOU ARE IN ROOM {room}",
            $"IT IS {GameData.GetRoomById(room).Description}",
    ];

    public static string[] TreasureDescriptions(Location[] locations) =>
        [.. locations.Select(x => $"IT CONTAINS {GameData.GetTreasureById(x.TreasureId).Name}"),
            string.Empty];
}
