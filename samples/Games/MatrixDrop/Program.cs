using Games.Common;
using Spectre.Console;

namespace MatrixDrop;

class Program
{
    private static readonly Random rnd = new();

    static void Main()
    {
        // Set console properties
        AnsiConsole.Cursor.Hide();

        int height = Console.WindowHeight - 1;
        int width = Console.WindowWidth/2;

        var rows = new List<List<MatrixCell>>();
        for (int y = 0; y < height; y++)
        {
            rows.Add(CreateRandomRow(width));
        }

        while (true)
        {
            rows.Insert(0, CreateRandomRow(width));

            var outputLines = AnsiConsole.Console.Render(rows, height);

            // Remove characters that have reached the bottom of the console window
            if (rows.Count >= height)
            {
                rows.RemoveAt(height - 1);
            }

            AnsiConsole.Console.Cursor.SetPosition(0, 0);
            AnsiConsole.Console.WriteMessage([.. outputLines]);
            Thread.Sleep(Constants.RefreshRate);
        }
    }

    static List<MatrixCell> CreateRandomRow(int width) =>
        Enumerable.Range(0, width).Select(x => GetRandomChar()).ToList();

    static MatrixCell GetRandomChar() => new()
    {
        Char = Constants.Chars[rnd.Next(Constants.Chars.Length)],
        Color = rnd.Next(1, 20) > 1 ? "darkgreen" : "lime"
    };
}
