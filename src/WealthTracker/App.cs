using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker;

internal static class App
{
    public static void Run(IAnsiConsole console)
    {
        var initialState = new AppState(console);
        initialState
            .Apply(x => DisplayTitle(console))
            .IterateUntil(
                x => NextCommand(x),
                x => x.CanContinue is false);
    }

    private static AppState NextCommand(AppState prevState)
    {
        prevState.Console.WriteLine();
        var inputCommand = prevState.Console.Ask<string>($"Enter conversion type (show or exit):");

        var result = CommandMetadata.GetCommandTypes()
            .Map(x => x.FirstOrDefault(t => t.AllowedCommands.Contains(inputCommand)))
            .Map(x => x ?? CommandMetadata.ErrorTypeHandler(inputCommand))
            .Map(x => x.TypeHandler(prevState, x));

        return result;
    }

    private static void DisplayTitle(IAnsiConsole console) =>
        console.Write(new FigletText("Wealth Tracker")
                        .Centered()
                        .Color(Color.Green));
}
