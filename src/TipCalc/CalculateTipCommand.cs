using D20Tek.Minimal.Functional;

namespace TipCalc;

internal static class CalculateTipCommand
{
    public static Maybe<TipResponse> Handle(TipRequest request) =>
        FunctionalExtensions.TryExcept<Maybe<TipResponse>>(
            () => CalcTip(request),
            ex => new Exceptional<TipResponse>(ex));

    private static TipResponse CalcTip(TipRequest request) =>
        CalcTipAmount(request.OriginalPrice, request.TipPercentage)
            .Map(tipAmount => (request.OriginalPrice + tipAmount)
                .Map(totalAmount => new TipResponse(tipAmount, totalAmount, totalAmount / request.TipperCount)));

    private static decimal CalcTipAmount(decimal price, decimal percentage) =>
        price * (percentage / Constants.Percent);
}
