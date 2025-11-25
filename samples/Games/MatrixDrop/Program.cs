using D20Tek.Functional;
using MatrixDrop;
using Spectre.Console;

TryExcept.Run(
    () =>
    {
        AnsiConsole.Cursor.Hide();
        Game.Play(AnsiConsole.Console, new Random());
    },
    ex => AnsiConsole.WriteException(ex),
    AnsiConsole.Cursor.Show);
