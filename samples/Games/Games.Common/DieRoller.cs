namespace Games.Common;

internal static class DieRoller
{
    private const int _min = 1;
    private const int _max = 7;
    private static readonly Random _rnd = new();

    public static int Roll() => 
        _rnd.Next(_min, _max);

    public static int Roll(int numSides) =>
        _rnd.Next(_min, numSides + 1);
}
