namespace GeneratePassword;

internal sealed record PasswordRequest(int Length, Config Config, Func<int, int> Rnd);
