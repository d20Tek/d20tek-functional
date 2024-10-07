namespace Zorgos;

internal sealed record RollResult(int[] Rolls, int ChipDelta, string Output)
{
    public static RollResult Empty => new([], 0, string.Empty);
}
