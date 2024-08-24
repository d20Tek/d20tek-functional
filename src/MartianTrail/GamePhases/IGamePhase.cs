using Spectre.Console;

namespace MartianTrail.GamePhases;

internal interface IGamePhase
{
    GameState DoPhase(GameState oldState);
}
