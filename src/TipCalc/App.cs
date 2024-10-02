using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace TipCalc;

internal static class App
{
    public static int Run(IAnsiConsole console) =>
        Identity<int>.Create(0)
            .Iter(_ => console.DisplayAppHeader(Constants.AppTitle))
            .Bind(_ => console.GatherTipRequest()
                .Map(request => (Request: request, Response: CalculateTipCommand.Handle(request)))
                .Iter(x => console.DisplayResult(x.Response, response => ShowTipResponseCommand.Handle(console, x.Request, response))))
                .Map(x => x.Response.ToResultCode());

    private static Identity<TipRequest> GatherTipRequest(this IAnsiConsole console) =>
        new TipRequest(console.GetOriginalPrice(), console.GetTipPercentage(), console.GetTipperCount());

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
