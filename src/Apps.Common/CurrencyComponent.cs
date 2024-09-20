using Spectre.Console;
using System.Globalization;

namespace Apps.Common;

internal static class CurrencyComponent
{
    public static string Render(decimal value) => $"{value:C}";

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
