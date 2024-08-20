using UnitConverter.Converters;

namespace UnitConverter;

internal sealed record ConvertTypeMetadata(
    string Name,
    string[] AllowedCommands,
    Func<ConverterState, ConvertTypeMetadata, ConverterState> TypeHandler,
    Dictionary<string, string> Units,
    IUnitConverter? Converter)
{
    public string[] GetUnitsList() => [.. Units.Keys];
}
