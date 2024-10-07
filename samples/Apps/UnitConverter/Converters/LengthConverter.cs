using D20Tek.Functional;

namespace UnitConverter.Converters;

internal class LengthConverter : IUnitConverter
{
    public Result<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        Result<decimal>.Success(value)
            .Bind(x => UnitToMeters(x, fromUnit))
            .Bind(x => MetersToUnit(x, toUnit));
        
    private static Result<decimal> UnitToMeters(decimal value, string unit) =>
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
            _ => IUnitConverter.InvalidUnitError(unit)
        };

    private static Result<decimal> MetersToUnit(decimal value, string unit) =>
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
            _ => IUnitConverter.InvalidUnitError(unit)
        };
}
