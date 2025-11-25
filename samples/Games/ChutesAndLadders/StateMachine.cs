using D20Tek.Functional;
using Spectre.Console;

namespace ChutesAndLadders;

internal static class StateMachine
{
    public static GameState InitialState(int numPlayers) => new(
        Players: Enumerable.Range(1, numPlayers)
                           .Select(x => new Player(Constants.StartingPosition, x, Constants.PlayerColors[x-1])),
        CurrentPlayer: Constants.StartingPlayer,
        NumberOfPlayers: numPlayers);

    public static GameState NextState(GameState state, Func<int> rollFunc) =>
        rollFunc().ToIdentity()
            .Map(r => new GameState(
                Players: state.Players.Select(x => x.Number == state.CurrentPlayer ? MovePlayer(x, r) : x).ToArray(),
                CurrentPlayer: NextPlayer(state.CurrentPlayer, state.NumberOfPlayers, r),
                NumberOfPlayers: state.NumberOfPlayers).Map(s =>
                    s with
                    {
                        LatestMove = $" - Move: P{state.CurrentPlayer} rolled {r} moved " +
                                     $"(from: {state.GetCurrentPlayer().Position}," +
                                     $" to: {s.GetPlayerById(state.CurrentPlayer).Position})"
                    })
                );

    private static Player MovePlayer(Player player, int roll) =>
        Math.Min(player.Position + roll, Constants.WinningPosition)
            .ToIdentity()
            .Map(pos => Constants.SnakesAndLadders.ContainsKey(pos) ? Constants.SnakesAndLadders[pos] : pos)
            .Map(pos => player with { Position = pos });

    private static int NextPlayer(int currentPlayer, int numPlayers, int roll) =>
        (roll == Constants.RerollValue)
            ? currentPlayer
            : Math.Clamp((currentPlayer + 1) % (numPlayers + 1), 1, numPlayers);
}
