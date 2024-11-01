using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace MatrixDrop;

internal static class MatrixPresenter
{
    public static void Render(this IAnsiConsole console, List<List<MatrixCell>> matrix, int height) =>
        console.ToIdentity()
               .Iter(c => c.Cursor.SetPosition(0, 0))
               .Iter(c => c.WriteMessage(matrix.RenderAsText(height)));

    private static string[] RenderAsText(this List<List<MatrixCell>> matrix, int height) =>
        matrix.Take(height)
              .Select(row => string.Join(" ", row))
              .ToArray();
}
