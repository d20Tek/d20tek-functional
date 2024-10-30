using D20Tek.Functional;
using FirePlace;
using Spectre.Console;

TryExcept.Run(
    () =>
    {
        AnsiConsole.Cursor.Hide();
        Game.Play(AnsiConsole.Console, Random.Shared.Next);
    },
    ex => AnsiConsole.WriteException(ex),
    () => AnsiConsole.Cursor.Show());

/*
using System.Text;

class RealisticFireSimulator
{
    private static readonly Random Random = new Random();
    private const int Width = 80;
    private const int Height = 24;
    private const int MaxHeat = 6;

    private static readonly int[,] FireMatrix = new int[Height, Width];
    private static readonly char[] FireChars = { ' ', '.', '*', '%', '#', '@' };

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        InitializeFireBase();

        while (true)
        {
            PropagateFire();
            RenderFire();
            Thread.Sleep(33);
        }
    }

    static void InitializeFireBase()
    {
        // Initialize the bottom row with high heat values for embers
        for (int x = 0; x < Width; x++)
        {
            FireMatrix[Height - 1, x] = Random.Next(3, MaxHeat);
        }
    }

    static void PropagateFire()
    {
        // Propagate heat upwards from the base
        for (int y = Height - 2; y >= 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                int decay = Random.Next(0, 2); // Randomly reduce heat
                int below = FireMatrix[y + 1, x];
                int newHeat = Math.Max(below - decay, 0);

                // Spread the heat slightly to the left and right for a more natural effect
                int offset = Random.Next(-1, 2);
                int xOffset = Math.Clamp(x + offset, 0, Width - 1);

                FireMatrix[y, xOffset] = newHeat;
            }
        }
    }

    static void RenderFire()
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int heat = FireMatrix[y, x];
                char fireChar = FireChars[heat];

                // Set color based on heat level
                Console.ForegroundColor = heat switch
                {
                    5 => ConsoleColor.DarkRed,
                    4 => ConsoleColor.Red,
                    3 => ConsoleColor.DarkYellow,
                    2 => ConsoleColor.Yellow,
                    1 => ConsoleColor.White,
                    _ => ConsoleColor.Black
                };

                Console.Write(fireChar);
            }
            Console.WriteLine();
        }
    }
}
*/
