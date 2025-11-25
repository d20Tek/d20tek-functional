using D20Tek.Functional;
using Spectre.Console;

namespace Games.Common;

internal static class AnsiConsoleExtensions
{
    public static void WriteMessage(this IAnsiConsole console, params string[] messages) =>
        console.MarkupLine(string.Join(Environment.NewLine, messages));

    public static void WriteMessage(this IAnsiConsole console, int offset, params string[] messages) =>
        _ = messages.Aggregate(
                console,
                (acc, message) =>
                {
                    acc.Cursor.MoveRight(offset);
                    acc.MarkupLine(message);
                    return acc;
                });

    public static void WriteMessageConditional(this IAnsiConsole console, bool condition, params string[] prompt) =>
        condition.IfTrueOrElse(() => console.WriteMessage(prompt));

    public static void PromptAnyKey(this IAnsiConsole console, string label) =>
        console.ToIdentity()
               .Iter(c => c.MarkupLine(label))
               .Iter(c => c.Input.ReadKey(true));

    public static void ContinueOnAnyKey(this IAnsiConsole console, string[] labels, int offset = 0) =>
        console.ToIdentity()
               .Iter(c => c.WriteMessage(offset, labels))
               .Iter(c => c.Input.ReadKey(true));

    public static void DisplayHeader(this IAnsiConsole console, string title, string color = "grey") =>
        console.ToIdentity()
               .Iter(x => x.WriteLine())
               .Iter(x => x.Write(new Rule(title).LeftJustified().RuleStyle(color)));

    public static void DisplayInstructions(
        this IAnsiConsole console,
        string showLabel,
        string[] instructions,
        string startLabel) =>
        console.Confirm(showLabel)
               .ToIdentity()
               .Iter(x => console.WriteMessageConditional(x, instructions))
               .Iter(x => x.IfTrueOrElse(() => console.PromptAnyKey(startLabel)));
}
