using D20Tek.Minimal.Functional;

namespace TipCalc;

internal static class Constants
{
    public const string AppTitle = "Tip Calc";
    public const decimal Percent = 100M;

    public const string OriginalPriceLabel = "Enter the [green]original price[/]:";
    public const string TipPercentageLabel = "Enter the [green]tip percentage[/]";
    public const string TipperCountLabel = "Enter the [green]number of people[/] splitting bill";

    public static readonly ValueRange<decimal> PercentRange = new(0, 100);
    public const string PercentRangeError = "Tip percentage must be between 0%-100%.";
    public static readonly ValueRange<decimal> TipperCountRange = new(1, 20);
    public const string TipperCountRangeError = "Number of tippers must be between 1-20.";

    public static int ToResultCode(this Maybe<TipResponse> maybe) => maybe is Something<TipResponse> ? 0 : -1;

    public static string[] TipResponseMessages(TipRequest request, TipResponse response) =>
    [
        string.Empty,
        $"[yellow]Original Price:[/] {request.OriginalPrice:C}",
        $"[yellow]Tip Percentage:[/] {request.TipPercentage}%",
        $"[yellow]Tip Amount:[/] {response.TipAmount:C}",
        $"[yellow]Total Amount:[/] {response.TotalAmount:C}",
        request.TipperCount > 1 ? $"[yellow]Amount Per Person:[/] {response.AmountPerTipper:C}" : string.Empty
    ];
}
