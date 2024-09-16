namespace GeneratePassword;

internal sealed record PasswordState(int Length, Config Config, Func<int, int> Rnd, string CharSet = "", double Entropy = 0);
