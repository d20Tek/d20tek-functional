using Crash;
using D20Tek.Functional;
using Spectre.Console;

TryExcept.Run(
    () =>
    {
        AnsiConsole.Cursor.Hide();
        Game.Play(AnsiConsole.Console, RandomGenerator.Roll);
    },
    ex => AnsiConsole.WriteException(ex),
    AnsiConsole.Cursor.Show);
