﻿using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace FirePlace;

internal static class Game
{
    public delegate int RndFunc(int min, int max);

    public static void Play(IAnsiConsole console, RndFunc rnd) =>
        Initialize(console, rnd)
            .Map(initialFrame =>
                initialFrame.IterateUntil(
                    x => x.NextFrame(console, rnd),
                    x => x.GameRunning is false))
            .Iter(x => console.WriteMessage(Constants.EndMessage));

    private static GameState Initialize(IAnsiConsole console, RndFunc rnd)
    {
        console.ShowStartMessage();
        return new(FireMatrix.Initialize(rnd));
    }

    private static void ShowStartMessage(this IAnsiConsole console) =>
        console.ToIdentity()
               .Iter(c => c.Clear())
               .Iter(c => c.WriteMessage(Constants.StartMessage))
               .Iter(c => c.PromptAnyKey(Constants.StartGameLabel));

    private static GameState NextFrame(this GameState state, IAnsiConsole console, RndFunc rnd) =>
        KeyboardInput.Handle(state, console)
            .Map(s => PropagateFire(s, rnd))
            .Iter(s => RenderFire(s, console))
            .Iter(s => Thread.Sleep(Constants.RefreshRate));

    private static GameState PropagateFire(GameState state, RndFunc rnd)
    {
        var matrix = state.FireMatrix.PropagateFire(rnd);
        return state with { FireMatrix = matrix };
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
