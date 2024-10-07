using D20Tek.Functional;

namespace UnitConverter.Converters;

internal class WeightConverter : IUnitConverter
{
    public Result<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        Result<decimal>.Success(value)
            .Bind(x => UnitToKilogramFactor(fromUnit)
                .Map(from => x * from))
            .Bind(x => UnitToKilogramFactor(toUnit)
                .Map(to => x / to));

    private static Result<decimal> UnitToKilogramFactor(string unit) =>
        unit.ToLower() switch
        {
            "kilogram" => 1m,
            "gram" => 0.001m,
            "milligram" => 0.000001m,
            "metric ton" => 1000m,
            "ton" => 907.185m, // US Ton
            "pound" => 0.45359237m,
            "ounce" => 0.0283495231m,
            _ => IUnitConverter.InvalidUnitError(unit)
        };
}
