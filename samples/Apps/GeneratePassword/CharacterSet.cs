namespace GeneratePassword;

internal static class CharacterSet
{
    private const string _lowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string _upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _numbers = "0123456789";
    private const string _symbols = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
    private const string _ambiguous = "iIl1L| o0O `'-_\":;.,";
    private const string _brackets = "<>()[]{}";

    public static string GetCharacterSet(this Config config) =>
        GetWorkingSet(CalculateFullSet(config), CalculateExclusions(config));

    public static IEnumerable<char> GetRequiredCharacters(this Config config, Func<int, int> rnd) =>
        new char?[]
        {
            config.IncludeLowerCase ? _lowerCase.GetRandomCharacter(rnd) : null,
            config.IncludeUpperCase ? _upperCase.GetRandomCharacter(rnd) : null,
            config.IncludeNumbers   ? _numbers.GetRandomCharacter(rnd)   : null,
            config.IncludeSymbols   ? _symbols.GetRandomCharacter(rnd)   : null
        }.Where(c => c.HasValue)
         .Select(c => c!.Value);

    public static char GetRandomCharacter(this string charSet, Func<int, int> rnd) =>
        charSet[rnd(charSet.Length)];

    private static string CalculateFullSet(Config config) =>
        string.Concat(
        [
            config.IncludeLowerCase? _lowerCase : string.Empty,
            config.IncludeUpperCase? _upperCase : string.Empty,
            config.IncludeNumbers? _numbers : string.Empty,
            config.IncludeSymbols? _symbols : string.Empty
        ]);

    private static HashSet<char> CalculateExclusions(Config config) =>
    [.. string.Concat(
        [
            config.ExcludeAmbiguous ? _ambiguous : string.Empty,
            config.ExcludeBrackets ? _brackets : string.Empty,
        ])
    ];

    private static string GetWorkingSet(string fullCharSet, HashSet<char> excludes) =>
        new(fullCharSet.Where(x => !excludes.Contains(x)).ToArray());
}
