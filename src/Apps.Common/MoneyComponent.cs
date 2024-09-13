using Spectre.Console;
using System.Globalization;

namespace Apps.Common;

internal static class MoneyComponent
{
    private const NumberStyles _numberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
    private const decimal _thousand = 1000M;
    private const decimal _million = 1_000_000M;
    private const decimal _billion = 1_000_000_000M;

    public static string Render(decimal value, string monetarySymbol = "$") =>
        $"{monetarySymbol} {value:N0}";

    public static string RenderShort(decimal value, string monetarySymbol = "$") =>
        value switch
        {
            > _billion => $"{monetarySymbol} {(value / _billion):0.##}B",
            > _million => $"{monetarySymbol} {(value / _million):0.##}M",
            > _thousand => $"{monetarySymbol} {(value / _thousand):0.##}K",
            _ => $"{monetarySymbol} {value:0.##}"
        };

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
