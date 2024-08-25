using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace MartianTrail.Common;

internal static class AnsiConsoleExtensions
{
    public static void WriteMessage(this IAnsiConsole console, params string[] messages) =>
        console.MarkupLine(string.Join(Environment.NewLine, messages));

    public static void WriteMessageConditional(this IAnsiConsole console, bool condition, params string[] prompt) =>
        condition.IfTrueOrElse(() => console.WriteMessage(prompt));

    public static void PromptAnyKey(this IAnsiConsole console, string label) =>
        console.Tap(c => c.MarkupLine(label))
               .Tap(c => c.Input.ReadKey(true));

    public static void DisplayHeader(this IAnsiConsole console, string title, string color = "grey") =>
        console.Tap(x => x.WriteLine())
               .Tap(x => x.Write(new Rule(title).LeftJustified().RuleStyle(color)));
}
