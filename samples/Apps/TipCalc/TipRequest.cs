namespace TipCalc;

internal sealed record TipRequest(decimal OriginalPrice, decimal TipPercentage, int TipperCount)
{
    public bool HasMultipleTippers() => TipperCount > Constants.MinimumTippers;
}
