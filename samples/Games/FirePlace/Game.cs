using D20Tek.Functional;
using Spectre.Console;
using System;

namespace FirePlace;

internal static class Game
{
    public delegate int RndFunc(int min, int max);

    public static void Play(IAnsiConsole console, RndFunc rnd)
    {
        Initialize(console, rnd)
            .Map(initialFrame =>
                initialFrame.IterateUntil(
                    x => x.NexState(console, rnd),
                    x => x.GameRunning is false))
            .Iter(x => console.WriteLine("Flame off..."));
    }

    private static GameState Initialize(IAnsiConsole console, RndFunc rnd)
    {
        var fireMatrix = new int[Constants.Height, Constants.Width];
        for (int x = 0; x < Constants.Width; x++)
        {
            fireMatrix[Constants.Height - 1, x] = rnd(3, Constants.MaxHeat);
        }

        console.Clear();
        return new(fireMatrix, console);
    }

    private static GameState NexState(this GameState state, IAnsiConsole console, RndFunc rnd) =>
        KeyboardInput.Handle(state, console)
            .Map(s => Update(s, rnd))
            .Iter(s => RenderFire(s, console))
            .Iter(s => Thread.Sleep(Constants.RefreshRate));

    private static GameState Update(GameState state, Game.RndFunc rnd)
    {
        for (int y = Constants.Height - 2; y >= 0; y--)
        {
            for (int x = 0; x < Constants.Width; x++)
            {
                int decay = rnd(0, 2); // Randomly reduce heat
                int below = state.FireMatrix[y + 1, x];
                int newHeat = Math.Max(below - decay, 0);

                // Spread the heat slightly to the left and right for a more natural effect
                int offset = rnd(-1, 2);
                int xOffset = Math.Clamp(x + offset, 0, Constants.Width - 1);

                state.FireMatrix[y, xOffset] = newHeat;
            }
        }

        return state;
    }

    static void RenderFire(GameState state, IAnsiConsole console)
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < Constants.Height; y++)
        {
            for (int x = 0; x < Constants.Width; x++)
            {
                int heat = state.FireMatrix[y, x];
                char fireChar = Constants.FireChars[heat];

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
