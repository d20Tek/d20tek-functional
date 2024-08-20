using D20Tek.Minimal.Functional;

namespace UnitConverter.Converters;

internal class TemperatureConverter : IUnitConverter
{
    public Maybe<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        new Something<decimal>(value)
            .Bind(x => UnitToCelsius(x, fromUnit))
            .Bind(x => CelsiusToUnit(x, toUnit));

    private static decimal UnitToCelsius(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "celsius" => value,
            "fahrenheit" => (value - 32) * 5 / 9,
            "kelvin" => value - 273.15M,
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };

    private static decimal CelsiusToUnit(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "celsius" => value,
            "fahrenheit" => (value * 9 / 5) + 32,
            "kelvin" => value + 273.15M,
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };
}
