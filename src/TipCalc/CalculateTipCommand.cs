using D20Tek.Minimal.Functional;

namespace TipCalc;

internal static class CalculateTipCommand
{
    public static Maybe<TipResponse> Handle(TipRequest request) =>
        FunctionalExtensions.TryExcept<Maybe<TipResponse>>(
            () => CalcTip(request),
            ex => new Exceptional<TipResponse>(ex));
    
    private static TipResponse CalcTip(TipRequest request)
    {
        var tipAmount = request.OriginalPrice * (request.TipPercentage / 100);
        var totalAmount = request.OriginalPrice + tipAmount;
        return new TipResponse(tipAmount, totalAmount, totalAmount / request.TipperCount);
    }
}
