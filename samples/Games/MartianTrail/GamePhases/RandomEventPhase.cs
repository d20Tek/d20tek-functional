using D20Tek.Functional;
using Games.Common;
using MartianTrail.Inventory;
using Spectre.Console;

namespace MartianTrail.GamePhases;

internal class RandomEventPhase : IGamePhase
{
    private readonly Func<int, int> _rnd;
    private readonly Func<int> _rndPercentage;
    private readonly IAnsiConsole _console;

    public RandomEventPhase(Func<int, int> rnd, IAnsiConsole console)
    {
        _rnd = rnd;
        _rndPercentage = () => rnd(100);
        _console = console;
    }

    public GameState DoPhase(GameState oldState) =>
        oldState.Map(s => GetRandomEvent() switch
        {
            Optional<RandomEventDetails> e when e.IsSome =>
                e.Get().Perform(new EventRequest(e.Get(), oldState, _console, _rnd)),
            _ => s
        });

    private Optional<RandomEventDetails> GetRandomEvent() =>
        GameCalculations.RandomEventOccurred(_rndPercentage) is false
            ? Optional<RandomEventDetails>.None()
            : Constants.RandomEvent.Events.Pipe(e => e[_rnd(e.Length)]);

    internal static GameState RecoverCredits(EventRequest request) =>
        GameCalculations.CalculateCreditsGained(request.Rnd)
            .ToIdentity()
            .Map(creditsFound => request.OldState with
            {
                LatestMoves = Constants.RandomEvent.EventMessages(request.Details, creditsFound),
                Inventory = request.OldState.Inventory with
                {
                    Credits = request.OldState.Inventory.Credits + creditsFound
                }
            });

    internal static GameState ExtraFoodFound(EventRequest request) =>
        GameCalculations.CalculateExtraFoodGained(request.Rnd)
            .ToIdentity()
            .Map(foodFound => request.OldState with
            {
                LatestMoves = Constants.RandomEvent.EventMessages(request.Details, foodFound),
                Inventory = request.OldState.Inventory with
                {
                    Food = request.OldState.Inventory.Food + foodFound
                }
            });

    internal static GameState YardSaleFound(EventRequest request) =>
        request.OldState
               .ToIdentity()
               .Iter(s => request.Console.WriteMessage(request.Details.Title))
               .Map(s => s with 
                {
                    Inventory = SelectInventoryCommand.YardSalePurchase(s.Inventory, request.Console)
                });

    internal static GameState AtmosphericEvent(EventRequest request) =>
        request.OldState.Inventory.AtmosphereSuits <= 0
            ? KillPlayer(request)
            : request.OldState with
            {
                LatestMoves = Constants.RandomEvent.EventMessages(request.Details),
                Inventory = request.OldState.Inventory with
                {
                    AtmosphereSuits = (request.OldState.Inventory.AtmosphereSuits - 
                                       GameCalculations.CalculateAtmosphereSuitsUsed(request.Rnd))
                                            .OrZero()
                }
            };

    internal static GameState ContractedIllness(EventRequest request) =>
        request.OldState.Inventory.MediPacks <= 0
            ? KillPlayer(request)
            : request.OldState with
              {
                  LatestMoves = Constants.RandomEvent.EventMessages(request.Details),
                  Inventory = request.OldState.Inventory with
                  {
                      MediPacks = (request.OldState.Inventory.MediPacks - 
                                   GameCalculations.CalculateMediPacksUsed(request.Rnd))
                                        .OrZero()
                  }
              };

    internal static GameState EnemiesAttack(EventRequest request) =>
        request.OldState.Inventory.LaserCharges <= 0
            ? KillPlayer(request)
            : request.OldState with
            {
                LatestMoves = Constants.RandomEvent.EventMessages(request.Details),
                Inventory = request.OldState.Inventory with
                {
                    LaserCharges = (request.OldState.Inventory.LaserCharges - 
                                    GameCalculations.CalculateChargesUsed(request.Rnd))
                                        .OrZero()
                }
            };

    private static GameState KillPlayer(EventRequest request) =>
        request.OldState with
        {
            LatestMoves = Constants.RandomEvent.EventErrorMessages(request.Details),
            PlayerIsDead = true
        };
}
