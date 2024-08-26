namespace MartianTrail.Common;

internal static class IntExtensions
{
    public static int OrZero(this int value) => value >= 0 ? value : 0;
}
