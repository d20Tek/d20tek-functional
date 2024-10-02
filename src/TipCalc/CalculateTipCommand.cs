using D20Tek.Functional;

namespace TipCalc;

internal static class CalculateTipCommand
{
    public static Result<TipResponse> Handle(TipRequest request) =>
        TryExcept.Run(
            () => Result<TipResponse>.Success(CalcTip(request)),
            ex => Result<TipResponse>.Failure(ex));

    private static TipResponse CalcTip(TipRequest request) =>
        CalcTipAmount(request.OriginalPrice, request.TipPercentage)
            .Bind(tipAmount => AddTotal(request.OriginalPrice, tipAmount)
                .Map(totalAmount => new TipResponse(tipAmount, totalAmount, totalAmount / request.TipperCount)));

    private static Identity<decimal> CalcTipAmount(decimal price, decimal percentage) =>
        price * (percentage / Constants.Percent);

    private static Identity<decimal> AddTotal(decimal price, decimal tipAmount) => price + tipAmount;
}
