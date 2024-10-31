using D20Tek.Functional;
using Games.Common;
using Spectre.Console;
using System.Text;

namespace FirePlace;

internal static class MatrixPresenter
{
    public static void RenderFire(this IAnsiConsole console, int[,] fireMatrix, FireConfig config) =>
        console.ToIdentity()
               .Iter(c => c.Cursor.SetPosition(0, 0))
               .Iter(c => c.WriteMessage(fireMatrix.AsString(config)));

    private static string AsString(this int[,] fireMatrix, FireConfig config) =>
        Enumerable.Range(0, config.Height)
            .Select(y => Enumerable.Range(0, config.Width)
                .Select(x => fireMatrix.RenderFlameChar(x, y))
                .Aggregate(new StringBuilder(), (builder, flameChar) => builder.Append(flameChar))
                .ToString())
            .Aggregate(new StringBuilder(), (builder, row) => builder.AppendLine(row))
            .ToString();

    private static string RenderFlameChar(this int[,] fireMatrix, int x, int y) =>
        fireMatrix[y, x].Pipe(heat => $"[{GetHeatColor(heat)}]{Constants.FireChars[heat]}[/]");

    private static string GetHeatColor(int heat) =>
        heat switch
        {
            5 => "maroon",
            4 => "red",
            3 => "olive",
            2 => "yellow",
            1 => "white",
            _ => "black"
        };
}
