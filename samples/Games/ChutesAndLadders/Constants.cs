namespace ChutesAndLadders;

internal static class Constants
{
    public const int RerollValue = 6;
    public const int StartingPlayer = 1;
    public const int StartingPosition = 1;
    public const int WinningPosition = 100;

    public static readonly Dictionary<int, int> SnakesAndLadders = new()
    {
        { 5, 18 },
        { 14, 29 },
        { 25, 47 },
        { 39, 3 },
        { 43, 62 },
        { 46, 11 },
        { 52, 31 },
        { 71, 91 },
        { 73, 58 },
        { 74, 96 },
        { 80, 40 },
        { 87, 32 },
        { 93, 70 },
        { 96, 79 },
        { 98, 6 }
    };

    public static readonly string[] PlayerColors =
    [
        "white",
        "yellow",
        "blue",
        "green",
        "orange3",
        "purple",
        "grey",
        "lime"
    ];
}
