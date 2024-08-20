using UnitConverter.Converters;

namespace UnitConverter;

internal static class ConverterMetadata
{
    public static Func<string[]> GetDisplayText = () =>
    [
        " - area: performs area conversions, like Square Meter to Square Foot.",
        " - length (len): performs length or distance conversions, like Kilometer to Mile.",
        " - temperature (temp): performs temperture conversions, like Celsius to Farenheit.",
        " - cubic-volume (cubic): performs cubic volume conversions, like Cubic Meter to Cubic Foot.",
        " - liquid-volume (volume): performs liquid volume conversions, like Liter to Gallon.",
        " - weight (wt): performs weight conversions, like Kilogram to Pound."
    ];

    private static Dictionary<string, string> _areaUnits = new()
    {
        { "Square Meter", "m^2" },
        { "Square Kilometer", "km^2" },
        { "Square Centimeter", "cm^2" },
        { "Square Millimeter", "mm^2" },
        { "Square Mile", "mi^2" },
        { "Square Yard", "yd^2" },
        { "Square Foot", "ft^2" },
        { "Square Inch", "in^2" }
    };

    private static Dictionary<string, string> _lengthUnits = new()
    {
        { "Meter", "m" },
        { "Kilometer", "km" },
        { "Centimeter", "cm" },
        { "Millimeter", "mm" },
        { "Mile", "mi" },
        { "Yard", "yd" },
        { "Foot", "ft" },
        { "Inch", "in" }
    };

    private static Dictionary<string, string> _cubicUnits = new()
    {
        { "Cubic Meter", "m^3" },
        { "Cubic Kilometer", "km^3" },
        { "Cubic Centimeter", "cm^3" },
        { "Cubic Millimeter", "mm^3" },
        { "Cubic Mile", "mi^3" },
        { "Cubic Yard", "yd^3" },
        { "Cubic Foot", "ft^3" },
        { "Cubic Inch", "in^3" }
    };

    private static Dictionary<string, string> _tempUnits = new()
    {
        { "Celsius", "°C" },
        { "Fahrenheit", "°F" },
        { "Kelvin", "°K" }
    };

    private static Dictionary<string, string> _volumeUnits = new()
    {
        { "Liter", "l" },
        { "Milliliter", "ml" },
        { "Gallon", "gal" },
        { "Quart", "qt" },
        { "Pint", "pt" },
        { "Cup", "c" },
        { "Fluid Ounce", "oz" },
        { "Tablespoon", "Ts" },
        { "Teaspoon", "ts" }
    };

    private static Dictionary<string, string> _weightUnits = new()
    {
        { "Kilogram", "kg" },
        { "Gram", "g" },
        { "Milligram", "mg" },
        { "Metric Ton", "mt" },
        { "Ton", "tons" },
        { "Pound", "lb" },
        { "Ounce", "oz" }
    };

    public static Func<ConvertTypeMetadata[]> GetConvertTypes = () =>
    [
        new ("show", ["show", "show-types"], CommandHandlers.ShowTypes, [], null),
        new ("area", ["area"], CommandHandlers.ConvertUnits, _areaUnits, new AreaConverter()),
        new ("length", ["len", "length"], CommandHandlers.ConvertUnits, _lengthUnits, new LengthConverter()),
        new ("temp", ["temp", "temperature"], CommandHandlers.ConvertUnits, _tempUnits, new TemperatureConverter()),
        new ("cubic", ["cubic", "cubic-volume"], CommandHandlers.ConvertUnits, _cubicUnits, new CubicConverter()),
        new ("volume", ["vol", "volume", "liquid-volume"], CommandHandlers.ConvertUnits, _volumeUnits, new VolumeConverter()),
        new ("weight", ["wt", "weight"], CommandHandlers.ConvertUnits, _weightUnits, new WeightConverter()),
        new ("exit", ["exit", "x"], CommandHandlers.Exit, [], null)
    ];

    public static ConvertTypeMetadata ErrorTypeHandler(string command) =>
        new("error", [], (x, t) => CommandHandlers.Error(x, command, t), [], null);
}
