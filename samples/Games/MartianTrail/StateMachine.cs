﻿using D20Tek.Functional;
using Games.Common;
using MartianTrail.GamePhases;
using MartianTrail.Inventory;
using Spectre.Console;

namespace MartianTrail;

internal static class StateMachine
{
    public static GameState InitialState(InventoryState initialInventory) =>
        new(
            DistanceTraveled: 0,
            DistanceThisTurn: 0,
            ReachedDestination: false,
            PlayerIsDead: false,
            CurrentSol: 0,
            UserActionSelectedThisTurn: PlayerActions.Unavailable,
            Inventory: initialInventory,
            LatestMoves: []);

    public static GameState NextTurn(GameState state, IAnsiConsole console, IGamePhase[] gamePhases) =>
        gamePhases.Aggregate(state, (acc, y) => ContinueTurn(acc, z => NextPhase(console, y, z)));

    private static GameState ContinueTurn(GameState state, Func<GameState, GameState> f) =>
        (state.ReachedDestination || state.PlayerIsDead) ? state : f(state);

    private static GameState NextPhase(IAnsiConsole console, IGamePhase phase, GameState oldState) => 
        phase.DoPhase(oldState)
             .Iter(x => console.WriteMessage(x.LatestMoves));
}
