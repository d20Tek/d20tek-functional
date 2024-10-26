namespace WealthTracker.Domain;

internal static class DomainConstants
{
    public static Exception FutureDateError(string propertyName) =>
        new ArgumentOutOfRangeException(propertyName, "Date value for updates cannot be in the future.");
}
