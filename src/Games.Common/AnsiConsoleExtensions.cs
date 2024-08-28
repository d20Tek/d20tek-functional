using D20Tek.Minimal.Functional;
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
        console.Apply(c => c.MarkupLine(label))
               .Apply(c => c.Input.ReadKey(true));

    public static void ContinueOnAnyKey(this IAnsiConsole console, string[] labels, int offset = 0) =>
        console.Apply(c => c.WriteMessage(offset, labels))
               .Apply(c => c.Input.ReadKey(true));

    public static void DisplayHeader(this IAnsiConsole console, string title, string color = "grey") =>
        console.Apply(x => x.WriteLine())
               .Apply(x => x.Write(new Rule(title).LeftJustified().RuleStyle(color)));
}
