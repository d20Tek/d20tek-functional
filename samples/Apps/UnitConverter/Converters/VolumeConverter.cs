using D20Tek.Functional;

namespace UnitConverter.Converters;

internal class VolumeConverter : IUnitConverter
{
    public Result<decimal> Convert(decimal value, string fromUnit, string toUnit) =>
        Result<decimal>.Success(value)
            .Bind(x => UnitToLiters(x, fromUnit))
            .Bind(x => LitersToUnit(x, toUnit));

    private static Result<decimal> UnitToLiters(decimal value, string unit) =>
        unit.ToLower() switch
        {
            "liter" => value,
            "milliliter" => value / 1000,
            "gallon" => value * 3.78541M,
            "quart" => value * 0.946353M,
            "pint" => value * 0.473176M,
            "cup" => value * 0.24M,
            "fluid ounce" => value * 0.0295735M,
            "tablespoon" => value * 0.0147868M,
            "teaspoon" => value * 0.00492892M,
            _ => IUnitConverter.InvalidUnitError(unit)
        };

    private static Result<decimal> LitersToUnit(decimal value, string unit) => 
        unit.ToLower() switch
        {
            "liter" => value,
            "milliliter" => value * 1000,
            "gallon" => value / 3.78541M,
            "quart" => value / 0.946353M,
            "pint" => value / 0.473176M,
            "cup" => value / 0.24M,
            "fluid ounce" => value / 0.0295735M,
            "tablespoon" => value / 0.0147868M,
            "teaspoon" => value / 0.00492892M,
            _ => IUnitConverter.InvalidUnitError(unit)
        };
}
