using D20Tek.Functional;

namespace MartianTrail.MiniGame;

internal sealed record MiniGameState(string UserInput, string TextToType, double TimeTaken) : IState
{
    public static MiniGameState Initialize(string textToType) => new(string.Empty, textToType, default);
}
