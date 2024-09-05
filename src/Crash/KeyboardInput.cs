using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace Crash;

internal static class KeyboardInput
{
    public static GameState Handle(GameState state, IAnsiConsole console) =>
        state.IterateUntil(
            s => ReadNextKey(console).Map(key =>
                key switch
                {
                    ConsoleKey.A or ConsoleKey.LeftArrow => s with { CarVelocity = Constants.Scene.VelocityLeft },
                    ConsoleKey.D or ConsoleKey.RightArrow => s with { CarVelocity = Constants.Scene.VelocityRight },
                    ConsoleKey.W or ConsoleKey.UpArrow or
                    ConsoleKey.S or ConsoleKey.DownArrow => s with { CarVelocity = Constants.Scene.VelocityStraight },
                    ConsoleKey.Escape => s with { GameRunning = false, KeepPlaying = false },
                    _ => s
                }),
            s => console.Input.IsKeyAvailable() is false);

    private static ConsoleKey ReadNextKey(IAnsiConsole console) =>
        console.Input.ReadKey(true)?.Key ?? ConsoleKey.None;
}
