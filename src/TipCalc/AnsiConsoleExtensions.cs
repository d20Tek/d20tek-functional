using D20Tek.Functional;
using Spectre.Console;

namespace Apps.Common;

internal static class AnsiConsoleExtensions
{
    public static void WriteMessage(this IAnsiConsole console, params string[] messages) =>
        console.MarkupLine(string.Join(Environment.NewLine, messages));

    public static void WriteMessageConditional(this IAnsiConsole console, bool condition, params string[] prompt) =>
        condition.IfTrueOrElse(() => console.WriteMessage(prompt));

    public static void ContinueOnAnyKey(this IAnsiConsole console, string[] labels) =>
        console.ToIdentity().Iter(c => c.WriteMessage(labels))
                            .Iter(c => c.Input.ReadKey(true));

    public static void DisplayHeader(this IAnsiConsole console, string title, string color = "grey", int width = 80) =>
        console.ToIdentity().Iter(x => x.WriteLine())
                            .Map(_ => new Rule(title).LeftJustified().RuleStyle(color))
                            .Iter(rule => console.Write(CreateSingleColumnGrid(width).AddRow(rule)));

    public static void DisplayAppHeader(this IAnsiConsole console, string title, Color? color = null) =>
        console.Write(new FigletText(title).Centered().Color(color ?? Color.Green));

    public static void DisplayOption<T>(this IAnsiConsole console, Option<T> option, Func<T, string[]> successMessage)
        where T : notnull =>
        console.WriteMessage(
            option switch
            {
                Some<T> s => successMessage(s),
                _ => ["Option value was not set."]
            });

    public static void DisplayResult<T>(this IAnsiConsole console, Result<T> result, Func<T, string[]> successMessage)
        where T: notnull =>
        console.WriteMessage(
            result switch
            {
                Success<T> s => successMessage(s),
                Failure<T> f => GetErrorMessages(f.GetErrors()),
                _ => ["An unexpected error occurred."]
            });

    public static void DisplayOption<T>(this IAnsiConsole console, Option<T> option, Action<T> successAction)
        where T : notnull
    {
        switch (option)
        {
            case Some<T> s:
                successAction(s);
                break;
            default:
                console.WriteMessage(["Option value was not set."]);
                break;
        };
    }

    public static void DisplayResult<T>(this IAnsiConsole console, Result<T> result, Action<T> successAction)
        where T : notnull
    {
        switch (result)
        {
            case Success<T> s:
                successAction(s);
                break;
            case Failure<T> f:
                console.WriteMessage(GetErrorMessages(f.GetErrors()));
                break;
            default:
                console.WriteMessage(["An unexpected error occurred."]);
                break;
        };
    }

    private static Grid CreateSingleColumnGrid(int width) =>
        new Grid().AddColumn(new GridColumn().NoWrap().Width(width));

    private static string[] GetErrorMessages(Error[] errors) =>
        errors.Select(x => $"[red]Error:[/] {x.Message}").ToArray();
}
