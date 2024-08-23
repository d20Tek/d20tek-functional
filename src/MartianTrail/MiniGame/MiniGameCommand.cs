﻿using D20Tek.Minimal.Functional;
using MartianTrail.Common;
using Spectre.Console;

namespace MartianTrail.MiniGame;

internal class MiniGameCommand
{
    private readonly IAnsiConsole _console;
    private readonly Func<int, int> _randFunc;
    private readonly TimeService _timeService;

    public MiniGameCommand(IAnsiConsole console, Func<int, int> randFunc, TimeService timeService)
    {
        _console = console;
        _randFunc = randFunc;
        _timeService = timeService;
    }

    public decimal Play() =>
        _console.Tap(c => c.DisplayHeader(Constants.MiniGame.Heading))
                .Tap(c => c.PromptAnyKey(Constants.MiniGame.StartMessage))
                .Map(c => GenerateRandomText(_randFunc))
                .Map(s => GetUserInput(_console, _timeService, s))
                .Map(s => CalculateAccuracy(s));

    private static MiniGameState GenerateRandomText(Func<int, int> randFunc) =>
        MiniGameState.Initialize(
            Enumerable.Repeat(0, Constants.MiniGame.NumberRandomChars)
                .Select(_ => randFunc(Constants.MiniGame.RandomCharMax))
                .Select(x => (char)(Constants.MiniGame.RandomCharBase + x))
                .Map(lettersToSelect => string.Join(string.Empty, lettersToSelect))
            );

    private static MiniGameState GetUserInput(IAnsiConsole console, TimeService timeService, MiniGameState oldState) =>
        timeService.MeasureDuration<string>(
            () => console.Ask<string>($"{Constants.MiniGame.UserInputLabel} {oldState.TextToType}"))
                .Map(x => oldState with { UserInput = x.Result, TimeTaken = x.TimeTaken });

    private static decimal CalculateAccuracy(MiniGameState state) =>
        state.UserInput.Map(i => i.Length == Constants.MiniGame.NumberRandomChars 
                                ? RateTextAccuracy(state.TextToType, i)
                                : Constants.MiniGame.NoAccuracy)
                       .Map(textAccuracy => textAccuracy * Constants.MiniGame.RateTimeAccuracy(state.TimeTaken));

    private static decimal RateTextAccuracy(string expected, string actual) =>
        expected.Zip(actual, (x, y) => char.ToUpper(x) == char.ToUpper(y))
            .Map(charByCharComparison => charByCharComparison.Sum(x => x ? 1 : 0))
            .Map(numCorrect => (decimal)numCorrect / Constants.MiniGame.NumberRandomChars);
}
