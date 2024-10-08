﻿using D20Tek.Functional;
using Games.Common;
using MartianTrail.GamePhases;
using MartianTrail.Inventory;
using MartianTrail.MiniGame;
using Spectre.Console;

namespace MartianTrail;

internal static class Game
{
    public static void Play(IAnsiConsole console, WebApiClient webApiClient, Func<int, int> rollFunc) =>
        InitializeGame(console)
            .Map(initialState => GetGamePhases(webApiClient, console, rollFunc)
                .ToIdentity()
                .Map(phases => initialState.IterateUntil(
                    x => StateMachine.NextTurn(x, console, phases),
                    x => x.PlayerIsDead || x.ReachedDestination)
                       .Iter(x => DisplayGameEnding(console, x))));

    private static GameState InitializeGame(IAnsiConsole console) =>
        console.ToIdentity()
               .Iter(x => DisplayTitle(x))
               .Iter(x => DisplayInstructions(x))
               .Map(x => SelectInventoryCommand.SelectInitialInventory(x))
               .Map(i => StateMachine.InitialState(i));

    private static void DisplayTitle(IAnsiConsole console) =>
        new FigletText(Constants.GameTitle)
            .Centered()
            .Color(Color.Green)
            .ToIdentity()
            .Iter(x => console.Write(x));

    private static void DisplayInstructions(IAnsiConsole console) =>
        console.Confirm(Constants.ShowInstructionsLabel)
            .ToIdentity()
            .Iter(x => console.WriteMessageConditional(x, Constants.Instructions));

    private static IGamePhase[] GetGamePhases(
        WebApiClient webApiClient,
        IAnsiConsole console,
        Func<int, int> rollFunc) =>
    [
        new DisplayMartianWeather(webApiClient),
        new SelectAction(rollFunc, console, new MiniGameCommand(console, rollFunc, new TimeService())),
        new RandomEventPhase(rollFunc, console),
        new UpdateProgress(rollFunc)
    ];

    private static void DisplayGameEnding(IAnsiConsole console, GameState finalState) =>
        console.MarkupLine(finalState switch
        {
            { ReachedDestination: true } => Constants.FinalDestinationMsg,
            { PlayerIsDead: true } => Constants.FinalPlayerDeadMsg,
            _ => Constants.FinalUnexpectedMsg
        });
}
