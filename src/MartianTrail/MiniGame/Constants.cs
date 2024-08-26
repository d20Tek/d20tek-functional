namespace MartianTrail;

internal static partial class Constants
{
    public static class MiniGame
    {
        public const string Heading = "Martian Mini-Game";
        public const string StartMessage = "Get ready, the mini-game is about to begin.\r\nPress any key to begin....";
        public const string UserInputLabel = "Please type the following letters as accurately as you can:";
        public const char RandomCharBase = 'A';
        public const int NumberRandomChars = 8;
        public const int RandomCharMax = 25;
        public const decimal NoAccuracy = 0M;

        public static decimal RateTimeAccuracy(double timeTaken) => 1M * (decimal)Math.Pow(0.9, timeTaken);
    }
}
