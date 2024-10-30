using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace FirePlace;

internal static class Game
{
    public delegate int RndFunc(int min, int max);

    public static void Play(IAnsiConsole console, RndFunc rnd) =>
        console.Initialize(rnd)
               .Map(initialFrame =>
                    initialFrame.IterateUntil(
                        x => x.NextFrame(console, rnd),
                        x => x.GameRunning is false))
               .Iter(x => console.WriteMessage(Constants.EndMessage));

    private static GameState Initialize(this IAnsiConsole console, RndFunc rnd) =>
        console.ToIdentity()
               .Iter(c => c.ShowStartMessage())
               .Map(_ => new GameState (FireMatrix.Initialize(Constants.FireConfig, rnd)));

    private static void ShowStartMessage(this IAnsiConsole console) =>
        console.ToIdentity()
               .Iter(c => c.Clear())
               .Iter(c => c.WriteMessage(Constants.StartMessage))
               .Iter(c => c.PromptAnyKey(Constants.StartGameLabel));

    private static GameState NextFrame(this GameState state, IAnsiConsole console, RndFunc rnd) =>
        KeyboardInput.Handle(state, console)
            .Map(s => UpdateFire(s, rnd))
            .Iter(s => console.RenderFire(s))
            .Iter(s => Thread.Sleep(Constants.RefreshRate));

    private static GameState UpdateFire(GameState state, RndFunc rnd) =>
        state with { FireMatrix = state.FireMatrix.PropagateFire(Constants.FireConfig, rnd) };

    static void RenderFire(this IAnsiConsole console, GameState state)
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < Constants.FireConfig.Height; y++)
        {
            for (int x = 0; x < Constants.FireConfig.Width; x++)
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
