using D20Tek.Functional;
using Spectre.Console;

namespace UnitConverter;

internal static class ConversionCommand
{
    public static void Handle(IAnsiConsole console, ConvertTypeMetadata metadata)
    {
        var from = GetSelectedUnit(console, "Select the unit to convert from:", metadata.GetUnitsList());
        var amount = GetConversionAmount(console, from);
        var to = GetSelectedUnit(console, "Select the unit to convert to:", metadata.GetUnitsList());
        var result = metadata.Converter is null
            ? Result<decimal>.Failure(
                Error.NotFound("Converter.NotFound", "The command doesn't have an associated converter."))
            : metadata.Converter.Convert(amount, from, to);

        console.MarkupLine(result switch
        {
            Success<decimal> c =>
                $"[green]Result:[/] {amount} {metadata.Units[from]} => {Math.Round(c, 10)} {metadata.Units[to]}",
            _ => $"[red]Error:[/] Could not find a converter from {from} to {to}. Please select again..."
        });
    }

    private static string GetSelectedUnit(IAnsiConsole console, string promptTitle, string[] startOptions)
    {
        var selectedUnit = console.Prompt(
            new SelectionPrompt<string>()
                .Title(promptTitle)
                .PageSize(10)
                .AddChoices(startOptions));

        console.WriteLine($"{promptTitle} {selectedUnit}");
        return selectedUnit;
    }

    private static decimal GetConversionAmount(IAnsiConsole console, string fromUnit) =>
        console.Ask<decimal>($"Enter amount in {fromUnit}:");
}
