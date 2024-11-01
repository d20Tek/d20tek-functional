using D20Tek.Functional;

namespace MatrixDrop;

internal sealed record GameState(
    List<List<MatrixCell>> Matrix,
    bool GameRunning = true) : IState;
