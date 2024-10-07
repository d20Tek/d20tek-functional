using Spectre.Console;

namespace MartianTrail.GamePhases;

internal sealed record EventRequest(
    RandomEventDetails Details,
    GameState OldState,
    IAnsiConsole Console,
    Func<int, int> Rnd);