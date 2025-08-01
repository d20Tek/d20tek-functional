using D20Tek.Functional;

namespace ChutesAndLadders;

internal record GameState(IEnumerable<Player> Players, int CurrentPlayer, int NumberOfPlayers, string LatestMove = "") : IState
{
    public Player GetCurrentPlayer() => GetPlayerById(CurrentPlayer);

    public Player GetPlayerById(int playerId) => Players.First(x => x.Number == playerId);

    public Optional<Player> GetWinningPlayer() =>
        Players.FirstOrDefault(x => x.IsWinner()).ToOptional();
}
