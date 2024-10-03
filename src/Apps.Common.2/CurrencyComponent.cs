using Spectre.Console;
using System.Globalization;

namespace Apps.Common;

internal static class CurrencyComponent
{
    private const decimal _thousand = 1000M;
    private const decimal _million = 1_000_000M;
    private const decimal _billion = 1_000_000_000M;

    public static string Render(decimal value) => $"{value:C}";

    public static string RenderWithNegative(decimal value) =>
        value >= 0 ? $"{value:C}"
                   : $"[red]{value:C}[/]";

    public static string RenderShort(decimal value) =>
        value switch
        {
            > _billion => $"{(value / _billion):0.##}B",
            > _million => $"{(value / _million):0.##}M",
            > _thousand => $"{(value / _thousand):0.##}K",
            _ => $"{value:0.##}"
        };

    public static decimal Input(IAnsiConsole console, string label, bool allowNegatives = true) =>
        console.Prompt(CreateTextPrompt(label, allowNegatives)).ToDecimal();

    public static decimal Input(IAnsiConsole console, string label, decimal prevValue, bool allowNegatives = true) =>
        console.Prompt(
            CreateTextPrompt(label, allowNegatives)
                .DefaultValue(Render(prevValue)))
                .ToDecimal();

    private static TextPrompt<string> CreateTextPrompt(string label, bool allowNegatives) =>
        new TextPrompt<string>(label)
            .Culture(CultureInfo.CurrentUICulture)
            .Validate(v =>
                (decimal.TryParse(v, NumberStyles.Currency, CultureInfo.CurrentUICulture, out decimal currency))
                    ? ValidateNegativeAllowed(currency, allowNegatives)
                    : ValidationResult.Error("[red]Invalid currency format.[/]"));

    private static ValidationResult ValidateNegativeAllowed(decimal value, bool allowNegatives) =>
        (allowNegatives is true || value >= 0)
            ? ValidationResult.Success()
            : ValidationResult.Error("[red]The currency amount cannot be negative.[/]");

    private static decimal ToDecimal(this string input) =>
        decimal.Parse(input, NumberStyles.Currency, CultureInfo.CurrentUICulture);
}
