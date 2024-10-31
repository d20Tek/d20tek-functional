namespace FirePlace;

internal static class Constants
{
    public static readonly FireConfig FireConfig = new(24, 80, 23, 6, 3);

    public const string StartMessage = "Flame on!";
    public const string StartGameLabel = "Press any key to start...";
    public const string EndMessage = "Flame off...";

    public static TimeSpan RefreshRate = TimeSpan.FromMilliseconds(50);

    public static readonly char[] FireChars = { ' ', '.', '*', '%', '#', '@' };

    public static string HeatColorMap(int heat) =>
        heat switch
        {
            5 => "maroon",
            4 => "red",
            3 => "olive",
            2 => "yellow",
            1 => "white",
            _ => "black"
        };
}
