using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace TipCalc;

internal static class App
{
    public static int Run(IAnsiConsole console) =>
        console.Apply(c => c.DisplayAppHeader(Constants.AppTitle))
               .Map(c => c.GatherTipRequest()
                    .Map(request => CalculateTipCommand.Handle(request)
                        .Apply(result => console.DisplayMaybe(
                            result,
                            response => ShowTipResponseCommand.Handle(console, request, response)))
                        .Map(result => result.ToResultCode())));

    private static TipRequest GatherTipRequest(this IAnsiConsole console) =>
        new (console.GetOriginalPrice(), console.GetTipPercentage(), console.GetTipperCount());

    private static decimal GetOriginalPrice(this IAnsiConsole console) =>
        console.Ask<decimal>(Constants.OriginalPriceLabel);

    private static decimal GetTipPercentage(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<decimal>(Constants.TipPercentageLabel)
                            .DefaultValue(15)
                            .Validate(v => Constants.PercentRange.InRange(v), Constants.PercentRangeError));

    private static int GetTipperCount(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.TipperCountLabel)
                            .DefaultValue(1)
                            .Validate(v => Constants.TipperCountRange.InRange(v), Constants.TipperCountRangeError));
}
