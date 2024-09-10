using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;
using WealthTracker.Commands;

namespace WealthTracker;

internal static class App
{
    public static void Run(IAnsiConsole console) =>
        AppState.Initialize(console)
            .Apply(x => console.Write(Presenters.AppHeader(Constants.AppTitle)))
            .IterateUntil(
                x => NextCommand(x),
                x => x.CanContinue is false);

    private static AppState NextCommand(AppState prevState) =>
        UserCommandInput(prevState.Console)
            .Map(inputCommand =>
                CommandMetadata.GetCommandTypes()
                    .Map(x => x.FirstOrDefault(t => t.AllowedCommands.Contains(inputCommand)))
                    .Map(x => x ?? CommandMetadata.ErrorTypeHandler(inputCommand))
                    .Map(x => x.TypeHandler(prevState, x))
            );

    private static string UserCommandInput(IAnsiConsole console) =>
        console.Apply(c => c.WriteLine())
               .Map(c => c.Ask<string>(Constants.AskCommandLabel));
}
