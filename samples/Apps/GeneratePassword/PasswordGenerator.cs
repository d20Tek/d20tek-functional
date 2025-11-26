using D20Tek.Functional;

namespace GeneratePassword;

internal static class PasswordGenerator
{
    public static Result<PasswordResponse> Handle(PasswordRequest request) =>
        Validate(request)
            .Map(GetCharacterSet)
            .Map(CalculateEntropy)
            .Map(state =>
                new PasswordResponse(
                    GetRandomCharacters(state).Shuffle(state.Rnd),
                    state.Entropy,
                    Constants.DetermineStrength(state.Entropy)));

    private static Result<PasswordState> Validate(PasswordRequest request) =>
        request switch
        {
            { Length: < 4 or > 64 } => Result<PasswordState>.Failure(Constants.PasswordLengthError),
            { Config: { IncludeLowerCase: false, IncludeUpperCase: false,
                        IncludeNumbers: false, IncludeSymbols: false } } =>
                Result<PasswordState>.Failure(Constants.PasswordNoCharSetsError),
            _ => Result<PasswordState>.Success(new PasswordState(request.Length, request.Config, request.Rnd))
        };

    private static PasswordState GetCharacterSet(PasswordState s) => s with { CharSet = s.Config.GetCharacterSet() };

    private static string GetRandomCharacters(PasswordState state) =>
        new (Enumerable
                .Range(0, state.Length - state.Config.RequiredCharsAmount)
                .Select(_ => state.CharSet.GetRandomCharacter(state.Rnd))
                .Concat(state.Config.GetRequiredCharacters(state.Rnd))
                .ToArray());

    private static string Shuffle(this string inputChars, Func<int, int> rnd) =>
        new([.. inputChars.ToCharArray().OrderBy(_ => rnd(int.MaxValue))]);

    private static PasswordState CalculateEntropy(PasswordState state) =>
        state with { Entropy = state.Length * Math.Log2(state.CharSet.Length) };
}