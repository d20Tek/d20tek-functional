using D20Tek.Functional;

namespace TipCalc;

internal static class Constants
{
    public const string AppTitle = "Tip Calc";
    public const decimal Percent = 100M;
    public const int MinimumTippers = 1;

    public const string OriginalPriceLabel = "Enter the [green]original price[/]:";
    public const string TipPercentageLabel = "Enter the [green]tip percentage[/]";
    public const string TipperCountLabel = "Enter the [green]number of people[/] splitting bill";

    public static readonly ValueRange<decimal> PercentRange = new(0, 100);
    public const string PercentRangeError = "Tip percentage must be between 0%-100%.";
    public static readonly ValueRange<decimal> TipperCountRange = new(1, 20);
    public const string TipperCountRangeError = "Number of tippers must be between 1-20.";

    public static int ToResultCode(this Result<TipResponse> result) => result.IsSuccess ? 0 : -1;

    public const int ColumnNameLen = 24;
    public const int ColumnAmountsLen = 10;
    public const string PriceRowLabel = "[yellow]Original Price:[/]";
    public const string TipPercentRowLabel = "[yellow]Tip Percentage:[/]";
    public const string TipAmountRowLabel = "[yellow]Tip Amount:[/]";
    public const string TotalAmountRowLabel = "[yellow]Total Amount:[/]";
    public const string AmountPerTipperLabel = "[yellow]Amount Per Person:[/]";

    public static string CurrencyDisplay(this decimal v) => $"{v:C}";
    
    public static string PercentageDisplay(this decimal p) => $"{p}%";
}
