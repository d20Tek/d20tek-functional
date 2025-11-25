using D20Tek.Functional;

namespace Crash;

internal sealed record GameState(
    char[,] Scene,
    int Score,
    int CarPosition,
    int CarVelocity,
    int PreviousRoadUpdate = 0,
    bool GameRunning = true,
    bool KeepPlaying = true,
    bool ConsoleSizeError = false) : IState
{
    public static GameState Empty() => new(new char[Constants.Width, Constants.Height], 0, 0, 0);
}
