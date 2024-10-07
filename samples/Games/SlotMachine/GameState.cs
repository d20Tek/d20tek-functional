using D20Tek.Functional;

namespace SlotMachine;

internal sealed record GameState(int Tokens, int Round = 1) : IState;
