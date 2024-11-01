using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace MatrixDrop;

internal static class Game
{
    public static void Play(IAnsiConsole console, Random rnd) =>
        Initialize(Console.WindowWidth / 2, Console.WindowHeight - 1, rnd)
               .Map(initialFrame =>
                    initialFrame.IterateUntil(
                        x => x.NextFrame(console, rnd),
                        x => x.GameRunning is false))
               .Iter(x => console.WriteMessage(Constants.EndMessage));

    private static GameState Initialize(int width, int height, Random rnd) =>
        new (Matrix.Initialize(width, height, rnd), width, height);

    private static GameState NextFrame(this GameState state, IAnsiConsole console, Random rnd) =>
        KeyboardInput.Handle(state, console)
            .Map(s => UpdateTopRow(s, rnd))
            .Iter(s => console.RenderMatrix(s.Matrix, s.Height))
            .Map(s => UpdateBottomRow(s))
            .Iter(s => Thread.Sleep(Constants.RefreshRate));

    private static GameState UpdateTopRow(GameState state, Random rnd) =>
        state with { Matrix = state.Matrix.AddTopRow(state.Width, rnd) };

    private static GameState UpdateBottomRow(GameState state) =>
        state with { Matrix = state.Matrix.RemoveBottomRow(state.Height) };
}
