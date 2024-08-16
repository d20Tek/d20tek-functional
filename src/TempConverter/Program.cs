using D20Tek.Minimal.Functional;
using Spectre.Console;
using TempConverter;

GetCommand(args)
    .Map(x => Run(x, AnsiConsole.Console))
    .Tap(op => AnsiConsole.MarkupLine(op()));

static string GetCommand(string[] args) =>
    args.FirstOrDefault() ?? "ftoc";

static Func<string> Run(string command, IAnsiConsole console) =>
    command.ToLower() switch
    {
        "ctof" => () => HandleCtoF(console),
        "ftoc" => () => HandleFtoC(console),
        _ => () => $"[red]Error:[/] '{command}' is an invalid command. Either use ftoc (Fahrenheit To Celcius) or ctof (Celcius to Fahrenheit)"
    };

static string HandleCtoF(IAnsiConsole console)
{
    var tempC = console.Ask<decimal>($"Enter degrees in Celcius:");
    var tempF = TemperatureConverter.CelciusToFahrenheit(tempC);
    return $"{tempC}°C => {tempF}°F";
}

static string HandleFtoC(IAnsiConsole console)
{
    var tempF = console.Ask<decimal>($"Enter degrees in Fahreneheit:");
    var tempC = TemperatureConverter.FahrenheitToCelcius(tempF);
    return $"{tempF}°F => {tempC}°C";
}