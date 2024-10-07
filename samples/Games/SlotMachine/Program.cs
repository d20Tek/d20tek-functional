using Games.Common;
using Spectre.Console;
using SlotMachine;

Game.Play(AnsiConsole.Console, DieRoller.Roll);

/*
var console = AnsiConsole.Console;

var tokens = Constants.StartingTokens;

while (tokens > 0 && tokens < 50)
{
    console.Clear();
    console.Write(Constants.Header);
    console.WriteMessage("", $"You have {tokens} tokens.");
    console.PromptAnyKey("Pull the arm...");

    var a = Constants.Fruit[DieRoller.Roll() - 1];
    var b = Constants.Fruit[DieRoller.Roll() - 1];
    var c = Constants.Fruit[DieRoller.Roll() - 1];
     
    console.WriteMessage("", $"    {a}    {b}     {c}", "");

    if (a == b && b == c && c == Constants.Cherry)
    {
        console.WriteMessage("Three cherries", "[green]You win the jackpot![/]");
        tokens += 20;
    }
    else if (a == b && b == c && c != Constants.Cherry)
    {
        console.WriteMessage("Three of a kind", "You win 5 tokens.");
        tokens += 5;
    }
    else if ((a == b && b != c) || (a == c && b != c) || (b == c && c != a))
    {
        console.WriteMessage("Two of a kind", "You win 2 tokens.");
        tokens += 2;
    }
    else
    {
        console.WriteMessage("No matches", "[yellow]You lose this round.[/]");
        tokens--;
    }

    console.PromptAnyKey($"<continue>");
}

console.WriteMessage(tokens <= 0 ? "[red]You lost all your tokens[/]" : "[green]You won![/]");
*/