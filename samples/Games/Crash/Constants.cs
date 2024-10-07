namespace Crash;

internal static class Constants
{
    public const int Width = 50;
    public const int Height = 30;
    public const int EndBannerHeight = 4;
    public static TimeSpan RefreshRate = TimeSpan.FromMilliseconds(33);

    public const string GameTitle = "Crash!";
    public const string ShowInstructionsLabel = "Would you like the game instructions?";
    public const string StartGameLabel = "Press any key to start...";
    public const string PlayAgainLabel = "Play again?";
    public const string EndGameMessage = "[green]Crash![/] game ended.";

    public static readonly string[] Instructions =
    [
        string.Empty,
        "This is a driving game.",
        "Stay on the road! Try not to crash!",
        string.Empty,
        "Use A, S, W, and D (or arrow keys) to control your speed.",
        string.Empty
    ];

    public static readonly string[] ConsoleSizeError =
    [
        "Terminal window is too small.",
        $"Minimum size is {Constants.Width} width x {Constants.Height} height.",
        "Increase the size of the console window.",
        string.Empty
    ];

    public static string[] ScoreMessage(int score) =>
    [
        $"Your score: [green]{score}[/]",
        "Game Over..."
    ];

    public static class Scene
    {
        public const int RoadWidth = 10;
        public const char FilledSpace = '.';
        public const char EmptySpace = ' ';
        public const char Crashed = 'X';
        public const char CarLeft = '<';
        public const char CarRight = '>';
        public const char CarStraight = '^';
        public const int ShiftRight = 1;
        public const int ShiftLeft = -1;
        public const int GoStraight = 0;

        public const int VelocityRight = 1;
        public const int VelocityLeft = -1;
        public const int VelocityStraight = 0;
    }

    public static int RandomizeRoadUpdate(this Game.RndFunc rnd, int previous) =>
        rnd(5) < 4 ? previous : rnd(3) - 1;
}
