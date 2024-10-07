using D20Tek.Functional;
using Spectre.Console;
using TempConverter;

GetCommand(args)
    .Map(x => Run(x, AnsiConsole.Console))
    .Iter(op => AnsiConsole.MarkupLine(op.DefaultValue("Invalid input, please try again.")));

static Option<string> GetCommand(string[] args) =>
    args.FirstOrDefault() ?? "ftoc";

static Option<string> Run(string command, IAnsiConsole console) =>
    command.ToLower() switch
    {
        "ctof" => HandleCtoF(console),
        "ftoc" => HandleFtoC(console),
        _ => $"[red]Error:[/] '{command}' is an invalid command. " +
             $"Either use ftoc (Fahrenheit To Celsius) or ctof (Celsius to Fahrenheit)"
    };

static Option<string> HandleCtoF(IAnsiConsole console) =>
    GetDegrees(console, "Enter degrees in Celsius:")
        .Map(tempC => (TempC: tempC, TempF: TemperatureConverter.CelsiusToFahrenheit(tempC).Get()))
        .Map(t => $"{t.TempC}°C => {t.TempF}°F");

static Option<string> HandleFtoC(IAnsiConsole console) => 
    GetDegrees(console, $"Enter degrees in Fahreneheit:")
        .Map(tempF => (TempC: TemperatureConverter.FahrenheitToCelsius(tempF).Get(), TempF: tempF))
        .Map(t => $"{t.TempF}°F => {t.TempC}°C");

static Option<decimal> GetDegrees(IAnsiConsole console, string label) =>
    console.Ask<decimal>(label);
