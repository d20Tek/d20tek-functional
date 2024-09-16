public class PasswordGenerator
{
    private static readonly Random _random = new();

    private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string Symbols = "!@#$%^&*()-_+=<>?";
    private const string Ambiguous = "{}[]()/\\'\"`~,;:.<>";

    internal static readonly string AllChars = LowerCase + UpperCase + Numbers + Symbols + Ambiguous;

    public static string GeneratePassword(int length)
    {
        if (length <= 0) throw new ArgumentException("Password length must be greater than 0.");

        var password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = GetRandomCharacter(AllChars);
        }

        return new string(password);
    }

    private static char GetRandomCharacter(string chars) => chars[_random.Next(chars.Length)];

    // Method to calculate entropy
    public static double CalculateEntropy(int passwordLength, int characterSetSize)
    {
        return passwordLength * Math.Log2(characterSetSize);
    }

    // Method to determine password strength based on entropy
    public static string DetermineStrength(double entropy)
    {
        return entropy switch
        {
            < 40 => "Very Weak",
            < 60 => "Weak",
            < 80 => "Good",
            < 120 => "Strong",
            _ => "Very Strong",
        };
    }
}

class Program
{
    static void Main(string[] args)
    {
        int passwordLength = 12; // Desired password length
        string password = PasswordGenerator.GeneratePassword(passwordLength);

        Console.WriteLine($"Generated Password: {password}");

        // Calculate entropy
        int charSetSize = PasswordGenerator.AllChars.Length;
        double entropy = PasswordGenerator.CalculateEntropy(passwordLength, charSetSize);
        Console.WriteLine($"Password Entropy: {entropy:0.###} bits");

        // Determine password strength
        string strength = PasswordGenerator.DetermineStrength(entropy);
        Console.WriteLine($"Password Strength: {strength}");
    }
}
