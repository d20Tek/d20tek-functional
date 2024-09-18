namespace TipCalc;

internal sealed record TipRequest(decimal OriginalPrice, decimal TipPercentage, int TipperCount);
