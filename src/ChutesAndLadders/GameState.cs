using D20Tek.Minimal.Functional;

namespace ChutesAndLadders;

internal record GameState(IEnumerable<Player> Players, int CurrentPlayer, int NumberOfPlayers, string LatestMove = "")
{
    public Player GetCurrentPlayer() => GetPlayerById(CurrentPlayer);

    public Player GetPlayerById(int playerId) => Players.First(x => x.Number == playerId);

    public Maybe<Player> GetWinningPlayer() =>
        Players.FirstOrDefault(x => x.IsWinner())
            .ToMaybeIfNull();
}
