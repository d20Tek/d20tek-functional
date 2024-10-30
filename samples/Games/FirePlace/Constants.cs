namespace FirePlace;

internal static class Constants
{
    public const int Width = 80;
    public const int Height = 24;
    public const int MaxHeat = 6;
    public const int EmberStart = 3;
    public const int BottomRow = Height - 1;

    public const string StartMessage = "Flame on!";
    public const string StartGameLabel = "Press any key to start...";
    public const string EndMessage = "Flame off...";

    public static TimeSpan RefreshRate = TimeSpan.FromMilliseconds(33);

    public static readonly char[] FireChars = { ' ', '.', '*', '%', '#', '@' };
}
