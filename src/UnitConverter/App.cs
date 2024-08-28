using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace UnitConverter;

internal static class App
{
    public static void Run(IAnsiConsole console)
    {
        var initialState = new ConverterState(console);
        initialState
            .Apply(x => DisplayTitle(console))
            .IterateUntil(
                x => NextCommand(x),
                x => x.CanContinue is false);
    }

    private static ConverterState NextCommand(ConverterState prevState)
    {
        prevState.Console.WriteLine();
        var inputCommand = prevState.Console.Ask<string>($"Enter conversion type (show or exit):");

        var result = ConverterMetadata.GetConvertTypes()
            .Map(x => x.FirstOrDefault(t => t.AllowedCommands.Contains(inputCommand)))
            .Map(x => x ?? ConverterMetadata.ErrorTypeHandler(inputCommand))
            .Map(x => x.TypeHandler(prevState, x));

        return result;
    }

    private static Action<IAnsiConsole> DisplayTitle = (IAnsiConsole console) =>
    {
        var title = new FigletText("Func Unit Convert")
                        .Centered()
                        .Color(Color.Green);

        console.Write(title);
    };
}
