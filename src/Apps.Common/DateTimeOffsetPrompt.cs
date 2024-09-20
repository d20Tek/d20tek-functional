using Spectre.Console;

namespace Apps.Common;

internal static class DateTimeOffsetPrompt
{
    public static DateTimeOffset GetDate(IAnsiConsole console, string label) =>
        console.Prompt(new TextPrompt<DateTimeOffset>(label));

    public static DateTimeOffset GetDate(IAnsiConsole console, string label, DateTimeOffset prevDate) =>
        console.Prompt(new TextPrompt<DateTimeOffset>(label)
                            .DefaultValue(prevDate)
                            .WithConverter(date => date.ToDateString()));

    public static DateTimeOffset GetPastDate(IAnsiConsole console, string label) =>
        console.Prompt(new TextPrompt<DateTimeOffset>(label)
            .Validate(d => d.IsFutureDate()
                        ? ValidationResult.Error("[red]Date cannot be in the future.[/]")
                        : ValidationResult.Success()));
}
