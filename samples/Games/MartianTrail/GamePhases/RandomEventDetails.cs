namespace MartianTrail.GamePhases;

internal sealed record RandomEventDetails(
    string Title,
    Func<int, string> SuccessMessage,
    string FailureMessage,
    Func<EventRequest, GameState> Perform);