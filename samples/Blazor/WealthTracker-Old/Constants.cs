namespace WealthTracker;

internal static class Constants
{
    public static Exception FutureDateError(string propertyName) =>
        new ArgumentOutOfRangeException(propertyName, "Date value for updates cannot be in the future.");
}
