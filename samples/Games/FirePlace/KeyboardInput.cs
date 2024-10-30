using D20Tek.Functional;
using Spectre.Console;

namespace FirePlace;

internal static class KeyboardInput
{
    public static GameState Handle(GameState state, IAnsiConsole console) =>
        state.IterateUntil(
            s => ReadNextKey(console).Map(key =>
                key switch
                {
                    ConsoleKey.Escape or ConsoleKey.Q => s with { GameRunning = false },
                    _ => s
                }),
            s => console.Input.IsKeyAvailable() is false);

    private static Identity<ConsoleKey> ReadNextKey(IAnsiConsole console) =>
        console.Input.ReadKey(true)?.Key ?? ConsoleKey.None;
}
