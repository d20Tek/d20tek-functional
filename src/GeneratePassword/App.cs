using Apps.Common;
using D20Tek.LowDb;
using D20Tek.Functional;
using Spectre.Console;

namespace GeneratePassword;

internal static class App
{
    public static int Run(string[] args, IAnsiConsole console, Func<int, int> rnd, LowDb<Config> configDb) =>
        GetPasswordLength(args)
            .Map(len => PasswordGenerator.Handle(new PasswordRequest(len, configDb.GetConfig(), rnd)))
            .Iter(result => console.DisplayResult(result, RenderResponse))
            .Map(result => result is Success<PasswordResponse> ? 0 : -1);

    private static Identity<int> GetPasswordLength(string[] args) =>
        int.TryParse(args.FirstOrDefault(), out int pwdLength) ? pwdLength : Constants.DefaultPasswordLength;

    private static string[] RenderResponse(PasswordResponse response) =>
    [
        Constants.PasswordMessage(response.Password.EscapeMarkup()),
        Constants.EntropyMessage(response.Entropy),
        Constants.StrengthMessage(response.Strength)
    ];
}
