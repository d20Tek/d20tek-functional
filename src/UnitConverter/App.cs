using D20Tek.Functional;
using Spectre.Console;

namespace UnitConverter;

internal static class App
{
    public static void Run(IAnsiConsole console) =>
        Identity<ConverterState>.Create(new ConverterState(console))
            .Iter(x => console.Write(GetDisplayTitle()))
            .IterateUntil(
                x => NextCommand(x),
                x => x.Get().CanContinue is false);

    private static Identity<ConverterState> NextCommand(Identity<ConverterState> prevState) =>
        prevState.Iter(s => s.Console.WriteLine())
                 .Map(s => FindMetadataType(s.Console.GetCommand(), ConverterMetadata.GetConvertTypes())
                    .Map(m => m.TypeHandler(s, m)));

    private static Identity<string> GetCommand(this IAnsiConsole console) =>
        console.Ask<string>($"Enter conversion type (show or exit):");

    private static Identity<ConvertTypeMetadata> FindMetadataType(string inputCommand, ConvertTypeMetadata[] types) =>
        types.FirstOrDefault(t => t.AllowedCommands.Contains(inputCommand))
            ?? ConverterMetadata.ErrorTypeHandler(inputCommand);

    private static FigletText GetDisplayTitle() =>
        new FigletText("Func Unit Convert")
            .Centered()
            .Color(Color.Green);
}
