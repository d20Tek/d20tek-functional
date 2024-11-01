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

        Game.Play(AnsiConsole.Console, rnd);

        //int height = Console.WindowHeight - 1;
        //int width = Console.WindowWidth/2;

        //var matrix = Matrix.Initialize(width, height, rnd);

        //while (true)
        //{
        //    matrix = matrix.AddTopRow(width, rnd);

        //    var outputLines = AnsiConsole.Console.Render(matrix, height);

        //    matrix = matrix.RemoveBottomRow(height);

        //    AnsiConsole.Console.Cursor.SetPosition(0, 0);
        //    AnsiConsole.Console.WriteMessage([.. outputLines]);
        //    Thread.Sleep(Constants.RefreshRate);
        //}

        AnsiConsole.Cursor.Show();
    }
}
