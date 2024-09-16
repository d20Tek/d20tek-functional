using D20Tek.Minimal.Functional;

namespace GeneratePassword;

internal static class PasswordGenerator
{
    public static Maybe<PasswordResponse> Handle(PasswordRequest request) =>
        Validate(request)
            .Bind(state => GetCharacterSet(state))
            .Bind(state => CalculateEntropy(state))
            .Bind(state =>
                new PasswordResponse(
                    GetRandomCharacters(state).Shuffle(state.Rnd),
                    state.Entropy,
                    Constants.DetermineStrength(state.Entropy)));

    private static Maybe<PasswordState> Validate(PasswordRequest request) =>
        request switch
        {
            { Length: < 4 or > 64 } => new Failure<PasswordState>(Constants.PasswordLengthError),
            { Config: { IncludeLowerCase: false, IncludeUpperCase: false,
                        IncludeNumbers: false, IncludeSymbols: false } } =>
                new Failure<PasswordState>(Constants.PasswordNoCharSetsError),
            _ => new PasswordState(request.Length, request.Config, request.Rnd)
        };

    private static PasswordState GetCharacterSet(PasswordState s) =>
        s with { CharSet = s.Config.GetCharacterSet() };

    private static string GetRandomCharacters(PasswordState state) =>
        new (Enumerable
                .Range(0, state.Length - state.Config.RequiredCharsAmount)
                .Select(_ => state.CharSet.GetRandomCharacter(state.Rnd))
                .Concat(state.Config.GetRequiredCharacters(state.Rnd))
                .ToArray());

    private static string Shuffle(this string inputChars, Func<int, int> rnd) =>
        new(inputChars.ToCharArray().OrderBy(_ => rnd(int.MaxValue)).ToArray());

    private static PasswordState CalculateEntropy(PasswordState state) =>
        state with { Entropy = state.Length * Math.Log2(state.CharSet.Length) };
}