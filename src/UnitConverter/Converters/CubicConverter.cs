using D20Tek.Functional;

namespace UnitConverter.Converters;

internal class CubicConverter : IUnitConverter
{
    public Result<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        Result<decimal>.Success(value)
            .Bind(x => ConvertToCubicMeters(x, fromUnit))
            .Bind(x => ConvertFromCubicMeters(x, toUnit));

    private static Result<decimal> ConvertToCubicMeters(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "cubic kilometer" => value * 1_000_000_000_000m,
            "cubic meter" => value,
            "cubic centimeter" => value / 1_000_000m,
            "cubic millimeter" => value / 1_000_000_000_000m,
            "cubic mile" => value * 4_168_181_825.44058m,
            "cubic yard" => value * 0.764554857984m,
            "cubic foot" => value * 0.028316846592m,
            "cubic inch" => value * 0.000016387064m,
            _ => IUnitConverter.InvalidUnitError(unit)
        };

    private static Result<decimal> ConvertFromCubicMeters(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "cubic kilometer" => value / 1_000_000_000_000m,
            "cubic meter" => value,
            "cubic centimeter" => value * 1_000_000m,
            "cubic millimeter" => value * 1_000_000_000_000m,
            "cubic mile" => value / 4_168_181_825.44058m,
            "cubic yard" => value / 0.764554857984m,
            "cubic foot" => value / 0.028316846592m,
            "cubic inch" => value / 0.000016387064m,
            _ => IUnitConverter.InvalidUnitError(unit)
        };
}
