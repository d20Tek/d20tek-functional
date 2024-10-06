using D20Tek.Functional;
using TreasureHunt.Data;

namespace TreasureHunt.Commands;

internal static class MoveCommands
{

    public static GameState North(GameState state) => Move(r => r.North, state);

    public static GameState East(GameState state) => Move(r => r.East, state);

    public static GameState South(GameState state) => Move(r => r.South, state);

    public static GameState West(GameState state) => Move(r => r.West, state);

    private static GameState Move(Func<Room, int> direction, GameState state) =>
        GameData.GetRoomById(state.CurrentRoom)
            .ToIdentity()
            .Map(r => direction(r))
            .Map(d => d == Constants.DirectionNotAllowed
                    ? state with { LatestMove = Constants.Commands.CannotMoveMessage }
                    : state with
                    {
                        CurrentRoom = d,
                        LatestMove = Constants.Commands.OkMessage
                    });
}
