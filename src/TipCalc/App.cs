using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace TipCalc;

// todo: add better error checking on inputs (use TextPrompt instead of Ask)
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

        return result is Something<TipResponse> ? 0 : -1;
    }

    private static TipRequest GatherTipRequest(this IAnsiConsole console) =>
        new (console.GetOriginalPrice(), console.GetTipPercentage(), console.GetTipperCount());

    private static decimal GetOriginalPrice(this IAnsiConsole console) =>
        console.Ask<decimal>(Constants.OriginalPriceLabel);

    private static decimal GetTipPercentage(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<decimal>(Constants.TipPercentageLabel)
                            .DefaultValue(15)
                            .Validate(v => Constants.PercentRange.InRange(v), "Tip percentage must be between 0%-100%."));

    private static int GetTipperCount(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.TipperCountLabel)
                            .DefaultValue(15)
                            .Validate(v => Constants.TipperCountRange.InRange(v), "Number of tippers must be between 1-20."));
}
