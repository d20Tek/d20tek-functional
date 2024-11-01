namespace MatrixDrop;

internal static class Constants
{
    public const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%&~!^*()-+={}\\|/?.,<>";

    public static TimeSpan RefreshRate = TimeSpan.FromMilliseconds(120);
    public const string EndMessage = "Unplugging from the Matrix...";

    public class Colors
    {
        public const string DimGreen = "darkgreen";
        public const string BrightGreen = "lime";
    }
}
