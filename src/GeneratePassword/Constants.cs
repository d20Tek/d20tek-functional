using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace GeneratePassword;

internal static class Constants
{
    public const int DefaultPasswordLength = 12;

    public static string PasswordMessage(string password) =>
        $"[yellow]Generated Password:[/] {password.EscapeMarkup()}";

    public static string EntropyMessage(double entropy) => $"[yellow]Password Entropy:[/] {entropy:0.##} bits";

    public static string StrengthMessage(string strength) => $"[yellow]Password Strength:[/] {strength}";

    public static readonly Error PasswordLengthError =
        Error.Validation("Password.Length", "Password length must be between 4-64 characters.");

    public static readonly Error PasswordNoCharSetsError =
        Error.Validation(
            "Password.CharacterSetsRequired",
            "Please include at least one characters set for the password to be based on.");

    public static string DetermineStrength(double entropy) =>
        entropy switch
        {
            < 40 => "Very Weak",
            < 50 => "Weak",
            < 60 => "Good",
            < 120 => "Strong",
            _ => "Very Strong",
        };
}
