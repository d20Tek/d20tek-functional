using D20Tek.Functional;
using Games.Common;

namespace MartianTrail.GamePhases;

internal sealed class UpdateProgress : IGamePhase
{
    private readonly Func<int> _batteriesUsedUp;
    private readonly Func<int> _foodUsedUp;

    public UpdateProgress(Func<int, int> rnd)
    {
        _batteriesUsedUp = () => rnd(4);
        _foodUsedUp = () => rnd(5) * 10;
    }

    public GameState DoPhase(GameState oldState) =>
        GameCalculations.DistanceTraveled(oldState)
            .ToIdentity()
            .Map(d => oldState with
                {
                    DistanceThisTurn = d,
                    DistanceTraveled = oldState.DistanceTraveled + d,
                    Inventory = oldState.Inventory with
                    {
                        Batteries = (oldState.Inventory.Batteries - _batteriesUsedUp()).OrZero(),
                        Food = (oldState.Inventory.Food - _foodUsedUp()).OrZero()
                    }
                })
            .Map(s => s with 
                {
                    ReachedDestination = s.DistanceTraveled >= Constants.UpdateProgress.CompletionDistance,
                    PlayerIsDead = s.PlayerIsDead ? s.PlayerIsDead : s.Inventory.Food <= 0,
                    LatestMoves = Constants.UpdateProgress.Display(s)
                });
}
