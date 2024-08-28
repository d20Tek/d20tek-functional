using D20Tek.Minimal.Functional;
using Games.Common;
using Spectre.Console;

namespace Zorgos;

internal class DicePresenter
{
    private static string[] _dieParts(int value) => [
         "┌───┐",
        $"│ {value} │", 
         "└───┘" ];

    public static void RenderRow(IAnsiConsole console, int[] rolls) =>
        (rolls.Length >= 0).IfTrueOrElse(() =>
        {
            console.WriteLine();
            RenderDie(console, rolls[0], 14);
            console.Cursor.MoveUp(3);
            RenderDie(console, rolls[1], 22);

        });

    public static void RenderDie(IAnsiConsole console, int value, int offset) =>
        console.WriteMessage(offset, _dieParts(value));
}
