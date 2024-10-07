using Spectre.Console;

namespace UnitConverter;

internal static class CommandHandlers
{
    public static ConverterState ShowTypes(ConverterState prevState, ConvertTypeMetadata metadata)
    {
        var newState = prevState with { Command = metadata.Name };

        newState.Console.WriteLine(
            string.Join(Environment.NewLine, ConverterMetadata.GetDisplayText()));

        return newState;
    }

    public static ConverterState ConvertUnits(ConverterState prevState, ConvertTypeMetadata metadata)
    {
        var newState = prevState with { Command = metadata.Name };
        ConversionCommand.Handle(newState.Console, metadata);
        return newState;
    }

    public static ConverterState Exit(ConverterState prevState, ConvertTypeMetadata metadata)
    {
        prevState.Console.MarkupLine("[green]Good-bye![/]");
        return prevState with { Command = metadata.Name, CanContinue = false };
    }

    public static ConverterState Error(ConverterState prevState, string command, ConvertTypeMetadata metadata)
    {
        prevState.Console.MarkupLine($"[red]Error:[/] The '{command}' conversion is unknown. Please select again...");
        return prevState with { Command = metadata.Name };
    }
}
