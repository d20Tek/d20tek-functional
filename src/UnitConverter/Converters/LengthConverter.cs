using D20Tek.Minimal.Functional;

namespace UnitConverter.Converters;

internal class LengthConverter : IUnitConverter
{
    public Maybe<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        new Something<decimal>(value)
            .Bind(x => UnitToMeters(x, fromUnit))
            .Bind(x => MetersToUnit(x, toUnit));
        
    private static decimal UnitToMeters(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "kilometer" => value * 1000,
            "meter" => value,
            "centimeter" => value / 100,
            "millimeter" => value / 1000,
            "mile" => value * 1609.344M,
            "yard" => value * 0.9144M,
            "foot" => value * 0.3048M,
            "inch" => value * 0.0254M,
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };

    private static decimal MetersToUnit(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "kilometer" => value / 1000,
            "meter" => value,
            "centimeter" => value * 100,
            "millimeter" => value * 1000,
            "mile" => value / 1609.344M,
            "yard" => value / 0.9144M,
            "foot" => value / 0.3048M,
            "inch" => value / 0.0254M,
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };
}
