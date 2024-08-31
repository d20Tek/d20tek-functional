using D20Tek.Minimal.Functional;
using TreasureHunt.Data;

namespace TreasureHunt.Commands;

internal static class InventoryCommands
{
    public static GameState Grab(GameState state) =>
        state.IsAlreadyCarryingTreasure()
            ? state with { LatestMove = Constants.Commands.CannotCarryMoreMessage }
            : state.GetTreasureInRoom(state.CurrentRoom)
                   .Map(treasureId => treasureId == Constants.NoTreasure
                        ? state with { LatestMove = Constants.Commands.EmptyRoomMessage }
                        : state with
                        {
                            TreasureLocations = state.RemoveTreasureFrom(treasureId),
                            Carrying = treasureId,
                            LatestMove = Constants.Commands.TreasureGrabbedMessage(
                                            GameData.GetTreasureById(treasureId).Name)
                        });

    public static GameState Put(GameState state) =>
        state.IsAlreadyCarryingTreasure() == false
            ? state with { LatestMove = Constants.Commands.CannotCarryMoreMessage }
            : state.Carrying
                   .Map(treasureId => state with
                   {
                       TreasureLocations = state.AddTreasureTo(treasureId, state.CurrentRoom),
                       Carrying = Constants.NoTreasure,
                       LatestMove = Constants.Commands.TreasurePlacedMessage(
                                        GameData.GetTreasureById(treasureId).Name,
                                        state.CurrentRoom)
                   });

    private static bool IsAlreadyCarryingTreasure(this GameState state) =>
        state.Carrying != Constants.NoTreasure;

    public static int GetTreasureInRoom(this GameState state, int room) =>
        state.TreasureLocations.First(x => x.Room == room).TreasureId;

    private static Location[] RemoveTreasureFrom(this GameState state, int treasureId) =>
        state.TreasureLocations.Select(x => x.TreasureId == treasureId
                                          ? new Location(x.TreasureId, Constants.NoRoom)
                                          : x).ToArray();

    private static Location[] AddTreasureTo(this GameState state, int treasureId, int room) =>
        state.TreasureLocations.Select(x => x.TreasureId == treasureId
                                          ? new Location(x.TreasureId, room)
                                          : x).ToArray();
}
