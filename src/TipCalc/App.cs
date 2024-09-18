using Apps.Common;
using Spectre.Console;

namespace TipCalc;

// todo: print out the tip results in a table

internal static class App
{
    public static int Run(IAnsiConsole console)
    {
        console.DisplayAppHeader(Constants.AppTitle);
        TipRequest request = console.GatherTipRequest();

        var result = CalculateTipCommand.Handle(request);
        console.DisplayMaybe(result, response => Constants.TipResponseMessages(request, response));
        //result.OnSomething(response => console.WriteMessage(Constants.TipResponseMessages(request, response)));

        return result.ToResultCode();
    }

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
