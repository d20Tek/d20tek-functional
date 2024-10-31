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
            .Iter(s => console.RenderFire(s.FireMatrix, Constants.FireConfig))
            .Iter(s => Thread.Sleep(Constants.RefreshRate));

    private static GameState UpdateFire(GameState state, RndFunc rnd) =>
        state with { FireMatrix = state.FireMatrix.PropagateFire(Constants.FireConfig, rnd) };
}
