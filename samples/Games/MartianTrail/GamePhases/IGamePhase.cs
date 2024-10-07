namespace MartianTrail.GamePhases;

internal interface IGamePhase
{
    GameState DoPhase(GameState oldState);
}
