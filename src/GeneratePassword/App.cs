using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace GeneratePassword;

internal static class App
{
    public static int Run(string[] args, IAnsiConsole console, Func<int, int> rnd) =>
        GetPasswordLength(args)
            .Map(len => PasswordGenerator.GeneratePassword(len, rnd))
            .Apply(result => result.Render(console, RenderResponse))
            .Map(result => result is Something<PasswordResponse> ? 0 : -1);

    private static int GetPasswordLength(string[] args) =>
        int.TryParse(args.FirstOrDefault(), out int pwdLength) ? pwdLength : Constants.DefaultPasswordLength;

    private static string[] RenderResponse(PasswordResponse response) =>
    [
        Constants.PasswordMessage(response.Password.EscapeMarkup()),
        Constants.EntropyMessage(response.Entropy),
        Constants.StrengthMessage(response.Strength)
    ];
}
