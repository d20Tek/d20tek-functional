using D20Tek.Minimal.Functional;

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
        CalculateDistanceTraveled(oldState)
            .Map(d => oldState with
                {
                    DistanceThisTurn = d,
                    DistanceTraveled = oldState.DistanceTraveled + d,
                    Inventory = oldState.Inventory with
                    {
                        Batteries = (oldState.Inventory.Batteries - _batteriesUsedUp()).Map(x => x >= 0 ? x : 0),
                        Food = (oldState.Inventory.Food - _foodUsedUp()).Map(x => x >= 0 ? x : 0)
                    }
                })
            .Map(s => s with 
                {
                    ReachedDestination = s.DistanceTraveled >= Constants.UpdateProgress.CompletionDistance,
                    PlayerIsDead = s.PlayerIsDead ? s.PlayerIsDead : s.Inventory.Food <= 0,
                    LatestMoves = Constants.UpdateProgress.Display(s)
                });

    private static int CalculateDistanceTraveled(GameState state) =>
        state.Inventory.Batteries * (state.UserActionSelectedThisTurn == PlayerActions.PushOn ? 100 : 50);
}
