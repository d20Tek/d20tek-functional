using Spectre.Console;

namespace MatrixDrop;

internal static class MatrixPresenter
{
    public static List<string> Render(this IAnsiConsole console, List<List<MatrixCell>> matrix, int height)
    {
        var outputLines = new List<string>();

        for (int y = 0; y < height; y++)
        {
            outputLines.Add(string.Join(" ", matrix[y]));
        }

        return outputLines;
    }
}
