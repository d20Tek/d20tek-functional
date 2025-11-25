using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace Crash;

internal static class ScenePresenter
{
    public static void Render(GameState state, IAnsiConsole console) =>
        SceneToText(state).Map(rows => string.Join(Environment.NewLine, rows))
                          .Iter(sceneText => console.Cursor.ResetPosition())
                          .Iter(console.Write);

    public static void ClearLines(int width, int height, IAnsiConsole console) => 
        EmptyLines(width, height).Iter(x => console.Cursor.ResetPosition())
                                 .Iter(console.Write);

    private static Identity<string[]> SceneToText(GameState state) =>
        Enumerable.Range(0, Constants.Height)
            .Reverse()
            .Select(i => Enumerable
                .Range(0, Constants.Width)
                .Select(j => (i == 1 && j == state.CarPosition) ? RenderCar(state) : state.Scene[i, j])
                .ToArray())
            .Select(row => new string(row))
            .ToArray();

    private static char RenderCar(GameState state) =>
        state switch
        {
            { GameRunning: false } => Constants.Scene.Crashed,
            { CarVelocity: < 0 } => Constants.Scene.CarLeft,
            { CarVelocity: > 0 } => Constants.Scene.CarRight,
            _ => Constants.Scene.CarStraight
        };

    private static Identity<string> EmptyLines(int width, int height) =>
        Enumerable.Range(0, height)
                  .Select(_ => new string(Constants.Scene.EmptySpace, width))
                  .Aggregate((line1, line2) => $"{line1}{Environment.NewLine}{line2}");
}
