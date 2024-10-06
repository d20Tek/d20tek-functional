using Spectre.Console;

namespace TreasureHunt;

internal static class Inputs
{
    public static string GetCommand(IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.CommandInputLabel)
                            .AddChoices(Constants.CommandOptions)
                            .HideChoices());
}
