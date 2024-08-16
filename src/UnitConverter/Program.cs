using Spectre.Console;

var displayTitle = () =>
{
    var title = new FigletText("Func Unit Convert")
                    .Centered()
                    .Color(Color.Green);

    AnsiConsole.Write(title);
};

var canContinue = true;
displayTitle();
while (canContinue)
{
    Console.WriteLine();
    var command = AnsiConsole.Ask<string>($"{Environment.NewLine}Enter conversion type (or exit):");
    switch (command)
    {
        case "show-types":
        case "show":
            AnsiConsole.WriteLine("temperature (temp): performs temperture conversions, like Celcius to Farenheit");
            break;
        case "temperature":
        case "temp":
            AnsiConsole.WriteLine("Perform temperature conversion...");
            break;
        case "exit":
        case "x":
            AnsiConsole.MarkupLine("[green]Good-bye![/]");
            canContinue = false;
            break;
        default:
            AnsiConsole.WriteLine($"The '{command}' conversion is unknown. Please select again...");
            break;
    }
}

/*
static decimal GatherInput(IAnsiConsole console, string inputUnits) =>
    console.Ask<decimal>($"Enter degrees in {inputUnits}:");

static decimal Convert(decimal input, Func<decimal, decimal> converter) =>
    converter(input);

static string FormatOutput(decimal input, string inputUnits, decimal output, string outputUnits) =>
    $"{input}{inputUnits} => {output}{outputUnits}";
*/