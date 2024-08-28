namespace MartianTrail;

internal static class RandomGenerator
{
    private const int _min = 0;
    private static readonly Random _rnd = new();

    public static int Roll(int max) =>
        _rnd.Next(_min, max);
}
