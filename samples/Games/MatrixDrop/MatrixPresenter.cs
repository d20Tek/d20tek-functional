using Spectre.Console;

namespace MatrixDrop;

internal static class MatrixPresenter
{
    public static List<string> Render(this IAnsiConsole console, List<List<MatrixCell>> matrix, int height) =>
        matrix.Take(height)
              .Select(row => string.Join(" ", row))
              .ToList();
}
