using D20Tek.Functional;

namespace UnitConverter.Converters;

internal class AreaConverter : IUnitConverter
{
    public Result<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        Result<decimal>.Success(value)
            .Bind(x => ConvertToSquareMeters(x, fromUnit))
            .Bind(x => ConvertFromSquareMeters(x, toUnit));

    private static Result<decimal> ConvertToSquareMeters(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "square kilometer" => value * 1_000_000m,
            "square meter" => value,
            "square centimeter" => value / 10_000m,
            "square millimeter" => value / 1_000_000m,
            "square mile" => value * 2_589_988.110336m,
            "square yard" => value * 0.83612736m,
            "square foot" => value * 0.09290304m,
            "square inch" => value * 0.00064516m,
            _ => IUnitConverter.InvalidUnitError(unit)
        };

    private static Result<decimal> ConvertFromSquareMeters(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "square kilometer" => value / 1_000_000m,
            "square meter" => value,
            "square centimeter" => value * 10_000m,
            "square millimeter" => value * 1_000_000m,
            "square mile" => value / 2_589_988.110336m,
            "square yard" => value / 0.83612736m,
            "square foot" => value / 0.09290304m,
            "square inch" => value / 0.00064516m,
            _ => IUnitConverter.InvalidUnitError(unit)
        };
}
