using D20Tek.Functional;

namespace UnitConverter.Converters;

public interface IUnitConverter
{
    public static Result<decimal> InvalidUnitError(string unit) =>
        Result<decimal>.Failure(Error.Invalid("Unit.Invalid", $"Invalid unit to convert ({unit})."));

    Result<decimal> Convert(decimal value, string fromUnit, string toUnit);
}
