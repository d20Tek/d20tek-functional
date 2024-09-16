using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace GeneratePassword;

internal static class MaybePresenter
{
    public static void Render(
        this Maybe<PasswordResponse> result,
        IAnsiConsole console,
        Func<PasswordResponse, string[]> successMessage) =>
            console.WriteMessage(
                result switch
                {
                    Something<PasswordResponse> s => successMessage(s),
                    Failure<PasswordResponse> e => GetErrorMessages(e.Error.Message),
                    Exceptional<PasswordResponse> e => GetErrorMessages(e.Message),
                    _ => Constants.UnexpectedErrorMesssage
                });

    private static string[] GetErrorMessages(string message) => [$"{Constants.ErrorLabel} {message}"];
}
