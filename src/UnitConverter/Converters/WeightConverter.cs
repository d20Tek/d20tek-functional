using D20Tek.Minimal.Functional;

namespace UnitConverter.Converters;

internal class WeightConverter : IUnitConverter
{
    public Maybe<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        new Something<decimal>(value)
            .Bind(x => x * UnitToKilogramFactor(fromUnit))
            .Bind(x => x / UnitToKilogramFactor(toUnit));

    private static decimal UnitToKilogramFactor(string unit) =>
        unit.ToLower() switch
        {
            "kilogram" => 1m,
            "gram" => 0.001m,
            "milligram" => 0.000001m,
            "metric ton" => 1000m,
            "ton" => 907.185m, // US Ton
            "pound" => 0.45359237m,
            "ounce" => 0.0283495231m,
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };
}
