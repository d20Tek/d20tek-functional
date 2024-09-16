namespace GeneratePassword;

internal sealed record PasswordResponse(string Password, double Entropy, string Strength);
