using D20Tek.Functional;
using FirePlace;
using Spectre.Console;

TryExcept.Run(
    () =>
    {
        AnsiConsole.Cursor.Hide();
        Game.Play(AnsiConsole.Console, Random.Shared.Next);
    },
    ex => AnsiConsole.WriteException(ex),
    AnsiConsole.Cursor.Show);
