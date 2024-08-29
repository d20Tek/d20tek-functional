using Spectre.Console;

namespace SlotMachine;

internal static class Constants
{
    private const string _gameTitle = "Slot Machine";
    public const string Cherry = "Cherry";
    public static string[] Fruit = ["Lemon", Cherry, "Melon", "Bell", "Grape", "Plum"];
    public const int StartingTokens = 10;
    public const int EndingTokens = 50;
    public const int NumColumns = 3;
    public const int a = 0;
    public const int b = 1;
    public const int c = 2;

    public const string PullLeverMessage = "Pull the arm...";
    public const string ContinueMessage = "<continue>";
    public static string[] JackpotMessage = ["Three cherries", "[green]You win the jackpot![/]"];
    public static string[] TripleMessage = ["Three of a kind", "You win 5 tokens."];
    public static string[] DoubleMessage = ["Two of a kind", "You win 2 tokens."];
    public static string[] LossMessage = ["No matches", "[yellow]You lose this round.[/]"];

    public static string[] GameLostMessage(int rounds) =>
        [string.Empty, $"[red]You lost all your tokens[/] in {rounds} rounds."];

    public static string[] GameWonMessage(int rounds) =>
        [string.Empty, $"[green]You won![/] in {rounds} rounds."];

    public static FigletText Header = new FigletText(_gameTitle).Color(Color.Green).Centered();

    public static string[] FruitRow(string[] items) =>
    [
        string.Empty,
        $"    {items[a]}    {items[b]}     {items[c]}",
        string.Empty
    ];

    public static string[] CurrentTokensMessage(int tokens) =>
        ["", $"You have {tokens} tokens."];
}
