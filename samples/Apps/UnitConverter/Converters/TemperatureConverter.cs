using D20Tek.Functional;

namespace UnitConverter.Converters;

internal class TemperatureConverter : IUnitConverter
{
    public Result<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        Result<decimal>.Success(value)
            .Bind(x => UnitToCelsius(x, fromUnit))
            .Bind(x => CelsiusToUnit(x, toUnit));

    private static Result<decimal> UnitToCelsius(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "celsius" => value,
            "fahrenheit" => (value - 32) * 5 / 9,
            "kelvin" => value - 273.15M,
            _ => IUnitConverter.InvalidUnitError(unit)
        };

    private static Result<decimal> CelsiusToUnit(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "celsius" => value,
            "fahrenheit" => (value * 9 / 5) + 32,
            "kelvin" => value + 273.15M,
            _ => IUnitConverter.InvalidUnitError(unit)
        };
}
