using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace Zorgos;

internal static class GatherBetCommand
{
    public static int Handle(IAnsiConsole console, int totalChips) =>
        console.ToIdentity()
               .Iter(c => c.Clear())
               .Iter(c => c.Cursor.SetPosition(Constants.TextLeftOffset + 1, Constants.TextTopOffset))
               .Iter(c => c.WriteMessage(Constants.TotalChipsLabel(totalChips)))
               .Iter(c => c.Cursor.MoveRight(Constants.TextLeftOffset))
               .Map(c => c.Prompt<int>(new TextPrompt<int>(Constants.PlaceBetLabel)
                            .DefaultValue(Constants.BetDefaultValue)
                            .Validate(x => x > 0 && x <= totalChips)));
}
