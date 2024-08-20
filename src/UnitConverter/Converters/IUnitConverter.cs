using D20Tek.Minimal.Functional;

namespace UnitConverter.Converters;

public interface IUnitConverter
{
    Maybe<decimal> Convert(decimal value, string fromUnit, string toUnit);
}
