using D20Tek.Minimal.Functional;

namespace GeneratePassword;

internal class PasswordGenerator
{
    private const string _lowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string _upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _numbers = "0123456789";
    private const string _symbols = "!@#$%^&*()-_+=<>?";
    private const string _ambiguous = "{}[]()/\\'\"`~,;:.<>";

    internal static readonly string _allChars = _lowerCase + _upperCase + _numbers + _symbols + _ambiguous;

    public static Maybe<PasswordResponse> GeneratePassword(int length, Func<int, int> rnd)
    {
        if (length < 4 || length > 30)
            return new Failure<PasswordResponse>(Constants.PasswordLengthError);

        var password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = GetRandomCharacter(_allChars, rnd);
        }

        var entropy = CalculateEntropy(length, _allChars.Length);
        return new PasswordResponse(new string(password), entropy, Constants.DetermineStrength(entropy));
    }

    private static char GetRandomCharacter(string chars, Func<int, int> rnd) => chars[rnd(chars.Length)];

    private static double CalculateEntropy(int passwordLength, int characterSetSize) =>
        passwordLength * Math.Log2(characterSetSize);
}