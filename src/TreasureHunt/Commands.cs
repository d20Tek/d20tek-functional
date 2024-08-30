using D20Tek.Minimal.Functional;

namespace TreasureHunt;

internal static class Commands
{
    public delegate GameState CommandFunc(GameState state);

    private static Dictionary<string, CommandFunc> _commands = new()
    {
        { "N", MoveNorth },
        { "E", MoveEast },
        { "S", MoveSouth },
        { "W", MoveWest },
    };

    public static CommandFunc FindCommand(string command) =>
        command.ToUpper()
            .Map(c => _commands.TryGetValue(c, out CommandFunc? value) ? value : None);

    private static GameState None(GameState state) =>
        state with { LatestMove = Constants.Commands.CommandError };

    private static GameState MoveNorth(GameState state) => Move(r => r.North, state);

    private static GameState MoveEast(GameState state) => Move(r => r.East, state);

    private static GameState MoveSouth(GameState state) => Move(r => r.South, state);

    private static GameState MoveWest(GameState state) => Move(r => r.West, state);

    private static GameState Move(Func<Room, int> direction, GameState state) =>
        GameData.GetRoomById(state.CurrentRoom)
            .Map(r => direction(r))
            .Map(d => d == 0
                    ? state with { LatestMove = Constants.Commands.CannotMoveMessage }
                    : state with
                      { 
                        CurrentRoom = d,
                        Moves = state.Moves + 1,
                        LatestMove = Constants.Commands.OkMessage
                      });
}
