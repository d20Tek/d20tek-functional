namespace MartianTrail.MiniGame;

internal sealed record MiniGameState(string UserInput, string TextToType, double TimeTaken)
{
    public static MiniGameState Initialize(string textToType) => new(string.Empty, textToType, default);
}