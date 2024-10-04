using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace Apps.Common;

internal static class AnsiConsoleExtensions_Obsolete
{
    public static void DisplayMaybe<T>(this IAnsiConsole console, Maybe<T> maybe, Func<T, string[]> successMessage) =>
        console.WriteMessage(
            maybe switch
            {
                Something<T> s => successMessage(s),
                Failure<T> e => GetErrorMessages(e.Error.Message),
                Exceptional<T> e => GetErrorMessages(e.Message),
                _ => GetErrorMessages("An unexpected error occurred.")
            });

    public static void DisplayMaybe<T>(this IAnsiConsole console, Maybe<T> maybe, Action<T> successAction)
    {
        switch (maybe)
        {
            case Something<T> s:
                successAction(s);
                break;
            case Failure<T> e:
                console.WriteMessage(GetErrorMessages(e.Error.Message));
                break;
            case Exceptional<T> e:
                console.WriteMessage(GetErrorMessages(e.Message));
                break;
            default:
                console.WriteMessage(GetErrorMessages("An unexpected error occurred."));
                break;
        };
    }

    private static string[] GetErrorMessages(string message) => [$"[red]Error:[/] {message}"];
}
