using Crash;
using Spectre.Console;

AnsiConsole.Cursor.Hide();
try
{
    Game.Play(AnsiConsole.Console, RandomGenerator.Roll);
}
finally
{
    AnsiConsole.Cursor.Show();
}
