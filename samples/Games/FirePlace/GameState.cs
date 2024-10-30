using D20Tek.Functional;

namespace FirePlace;

internal sealed record GameState(
    int[,] FireMatrix,
    bool GameRunning = true) : IState
{
    public static GameState Empty() =>
        new(new int[Constants.Width, Constants.Height]);
}
