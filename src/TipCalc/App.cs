using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace TipCalc;

internal static class App
{
    public static int Run(IAnsiConsole console)
    {
        TipRequest request = console.GatherTipRequest();

        var result = CalculateTipCommand.Handle(request);
        result.OnSomething(response => console.PrintTipReponse(request, response));

        return 0;
    }

    private static TipRequest GatherTipRequest(this IAnsiConsole console) =>
        new (
            console.Ask<decimal>("Enter the [green]original price[/]:"),
            console.Ask<decimal>("Enter the [green]tip percentage[/]:"),
            console.Ask<int>("Enter the [green]number of people[/] splitting bill:"));

    private static void PrintTipReponse(this IAnsiConsole console, TipRequest request, TipResponse response)
    {
        console.WriteLine();
        console.MarkupLine($"[yellow]Original Price:[/] {request.OriginalPrice:C}");
        console.MarkupLine($"[yellow]Tip Percentage:[/] {request.TipPercentage}%");
        console.MarkupLine($"[yellow]Tip Amount:[/] {response.TipAmount:C}");
        console.MarkupLine($"[yellow]Total Amount:[/] {response.TotalAmount:C}");

        if (request.TipperCount > 1)
        {
            console.MarkupLine($"[yellow]Amount Per Person:[/] {response.AmountPerTipper:C}");
        }
    }
}
