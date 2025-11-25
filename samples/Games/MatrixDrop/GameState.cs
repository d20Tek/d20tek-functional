using D20Tek.Functional;

namespace MatrixDrop;

internal sealed record GameState(List<List<MatrixCell>> Matrix, int Width, int Height, bool GameRunning = true) :
    IState;
