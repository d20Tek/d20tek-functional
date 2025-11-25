using D20Tek.Functional;
using Games.Common;
using Spectre.Console;

namespace MartianTrail.MiniGame;

internal class MiniGameCommand(IAnsiConsole console, Func<int, int> randFunc, TimeService timeService)
{
    private readonly IAnsiConsole _console = console;
    private readonly Func<int, int> _randFunc = randFunc;
    private readonly TimeService _timeService = timeService;

    public decimal Play() =>
        GenerateRandomText(_randFunc)
            .Iter(s => _console.DisplayHeader(Constants.MiniGame.Heading))
            .Iter(s => _console.PromptAnyKey(Constants.MiniGame.StartMessage))
            .Map(s => GetUserInput(_console, _timeService, s))
            .Map(CalculateAccuracy);

    private static MiniGameState GenerateRandomText(Func<int, int> randFunc) =>
        MiniGameState.Initialize(
            Enumerable.Repeat(0, Constants.MiniGame.NumberRandomChars)
                .Select(_ => randFunc(Constants.MiniGame.RandomCharMax))
                .Select(x => (char)(Constants.MiniGame.RandomCharBase + x))
                .ToIdentity()
                .Map(lettersToSelect => string.Join(string.Empty, lettersToSelect))
            );

    private static MiniGameState GetUserInput(IAnsiConsole console, TimeService timeService, MiniGameState oldState) =>
        timeService.MeasureDuration<string>(
            () => console.Prompt<string>(
                    new TextPrompt<string>($"{Constants.MiniGame.UserInputLabel} {oldState.TextToType} - ")
                        .Secret('?')))
                .ToIdentity()
                .Map(x => oldState with { UserInput = x.Result, TimeTaken = x.TimeTaken });

    private static decimal CalculateAccuracy(MiniGameState state) =>
        state.UserInput
             .ToIdentity()
             .Map(i => RateTextAccuracy(state.TextToType, i))
             .Map(textAccuracy => textAccuracy * Constants.MiniGame.RateTimeAccuracy(state.TimeTaken));

    private static decimal RateTextAccuracy(string expected, string actual) =>
        actual.Length != Constants.MiniGame.NumberRandomChars
                      ? Constants.MiniGame.NoAccuracy
                      : expected.Zip(actual, (x, y) => char.ToUpper(x) == char.ToUpper(y))
                                .ToIdentity()
                                .Map(charByCharComparison => charByCharComparison.Sum(x => x ? 1 : 0))
                                .Map(numCorrect => (decimal)numCorrect / Constants.MiniGame.NumberRandomChars);
}
