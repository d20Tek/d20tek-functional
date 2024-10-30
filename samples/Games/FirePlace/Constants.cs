namespace FirePlace;

internal static class Constants
{
    public const int Width = 80;
    public const int Height = 24;
    public const int MaxHeat = 6;

    public static TimeSpan RefreshRate = TimeSpan.FromMilliseconds(33);

    public static readonly char[] FireChars = { ' ', '.', '*', '%', '#', '@' };
}
