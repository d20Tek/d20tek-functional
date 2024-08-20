using D20Tek.Minimal.Functional;

namespace UnitConverter.Converters;

internal class CubicConverter : IUnitConverter
{
    public Maybe<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        new Something<decimal>(value)
            .Bind(x => ConvertToCubicMeters(x, fromUnit))
            .Bind(x => ConvertFromCubicMeters(x, toUnit));

    private static decimal ConvertToCubicMeters(decimal value, string unit) =>
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
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };

    private static decimal ConvertFromCubicMeters(decimal value, string unit) =>
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
            _ => throw new ArgumentException($"Invalid unit to convert ({unit})."),
        };
}
