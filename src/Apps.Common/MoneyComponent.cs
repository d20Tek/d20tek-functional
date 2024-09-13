using Spectre.Console;
using System.Globalization;

namespace Apps.Common;

internal static class MoneyComponent
{
    private const NumberStyles _numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

    public static string Render(decimal value, string monetarySymbol = "$") =>
        $"{monetarySymbol} {value:N0}";

    public static decimal Input(IAnsiConsole console, string label) =>
        console.Prompt(CreateTextPrompt(label)).ToDecimal();

    private static TextPrompt<string> CreateTextPrompt(string label) =>
        new TextPrompt<string>(label)
            .Culture(CultureInfo.CurrentUICulture)
            .Validate(v =>
                (decimal.TryParse(v, _numberStyles, CultureInfo.CurrentUICulture, out _))
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Invalid number format.[/]"));

    private static decimal ToDecimal(this string input) =>
        decimal.Parse(input, NumberStyles.Number, CultureInfo.CurrentUICulture);
}
