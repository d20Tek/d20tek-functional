using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;

namespace Zorgos;

internal static class GatherBetCommand
{
    public static int Handle(IAnsiConsole console, int totalChips) =>
        console.Apply(c => c.Clear())
               .Apply(c => c.Cursor.SetPosition(Constants.TextLeftOffset + 1, Constants.TextTopOffset))
               .Apply(c => c.WriteMessage(Constants.TotalChipsLabel(totalChips)))
               .Apply(c => c.Cursor.MoveRight(Constants.TextLeftOffset))
               .Map(c => c.Prompt<int>(new TextPrompt<int>(Constants.PlaceBetLabel)
                            .DefaultValue(Constants.BetDefaultValue)
                            .Validate(x => x > 0 && x <= totalChips)));
}
