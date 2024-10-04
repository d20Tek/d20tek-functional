using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;
using WealthTracker.Commands;

namespace WealthTracker;

internal static class App
{
    public static void Run(IAnsiConsole console, IWealthRepository wealthRepo) =>
        AppState.Initialize(console, wealthRepo)
            .Iter(x => console.DisplayAppHeader(Constants.AppTitle))
            .IterateUntil(
                x => NextCommand(x),
                x => x.CanContinue is false);

    private static AppState NextCommand(AppState prevState) =>
        prevState.Console.GetUserCommandInput()
            .Bind(inputCommand => FindMetadataType(inputCommand, Configuration.GetCommandTypes()))
            .Map(m => m.TypeHandler(prevState, m));

    private static Identity<string> GetUserCommandInput(this IAnsiConsole console) =>
        console.ToIdentity()
               .Iter(c => c.WriteLine())
               .Map(c => c.Prompt(new TextPrompt<string>(Constants.AskCommandLabel)
                            .AddChoices(Configuration.GetCommands())
                            .ShowChoices(false)));

    private static Identity<CommandTypeMetadata> FindMetadataType(string inputCommand, CommandTypeMetadata[] types) =>
        types.FirstOrDefault(t => t.AllowedCommands.Contains(inputCommand))
            ?? Configuration.ErrorTypeHandler(inputCommand);
}
