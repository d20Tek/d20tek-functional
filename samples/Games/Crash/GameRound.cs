using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace Crash;

internal static class GameRound
{
    public static GameState PlayRound(GameState state, IAnsiConsole console, Game.RndFunc rnd) =>
        InitializeScene(state)
            .Map(initialSceneState =>
                initialSceneState.IterateUntil(
                    x => x.NexState(console, rnd),
                    x => x.GameRunning is false)
            .Map(s => RoundOverScreen(s, console.ToIdentity())));

    private static GameState NexState(this GameState state, IAnsiConsole console, Game.RndFunc rnd) =>
        (ConsoleSizer.IsValidSize(Constants.Width, Constants.Height) is false).IfTrueOrElse(
            () => state with { ConsoleSizeError = true, KeepPlaying = false },
            () => KeyboardInput.Handle(state, console)
                    .Map(s => Update(s, rnd))
                    .Iter(s => ScenePresenter.Render(s, console))
                    .Iter(s => Thread.Sleep(Constants.RefreshRate)));

    private static GameState InitializeScene(GameState state) =>
        state with
        {
            Score = 0,
            GameRunning = true,
            CarPosition = Constants.Width / 2,
            CarVelocity = 0,
            Scene = GameScene.Create(),
        };

    private static GameState Update(GameState state, Game.RndFunc rnd) =>
        GameScene.Update(state.Scene, state.PreviousRoadUpdate, rnd)
                 .Map(response => response with { NewCarPosition = state.CarPosition + state.CarVelocity })
                 .Map(r => state with
                 {
                    Scene = r.Scene,
                    PreviousRoadUpdate = r.RoadUpdate,
                    CarPosition = r.NewCarPosition,
                    GameRunning = GameScene.ShouldContinueRunning(r.Scene, r.NewCarPosition),
                    Score = state.Score + 1
                 });

    private static GameState RoundOverScreen(GameState state, Identity<IAnsiConsole> console) =>
        (state.KeepPlaying is false)
            ? state
            : console.Iter(c => ScenePresenter.ClearLines(Constants.Width, Constants.EndBannerHeight, c))
                     .Iter(c => c.Cursor.ResetPosition())
                     .Iter(c => c.WriteMessage(Constants.ScoreMessage(state.Score)))
                     .Map(c => state with { KeepPlaying = c.Confirm(Constants.PlayAgainLabel) });
}
