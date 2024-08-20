using D20Tek.Minimal.Functional;

namespace TempConverter;

internal static class TemperatureConverter
{
    public static decimal CelsiusToFahrenheitImp(decimal tempC)
    {
        decimal tempF = (tempC * 9 / 5) + 32;
        var result = Math.Round(tempF, 2);
        return result;
    }

    public static decimal CelsiusToFahrenheit(decimal tempC)
    {
        var result = 
            tempC.Map(x => x * 9)
                 .Map(x => x / 5)
                 .Map(x => x + 32)
                 .Map(x => Math.Round(x, 2));

        return result;
    }

    public static decimal FahrenheitToCelsius(decimal tempF)
    {
        var result = 
            tempF.Map(x => x - 32)
                 .Map(x => x * 5)
                 .Map(x => x / 9)
                 .Map(x => Math.Round(x, 2));

        return result;
    }
}
