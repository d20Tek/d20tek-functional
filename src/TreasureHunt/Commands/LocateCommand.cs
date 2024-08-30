using TreasureHunt.Data;

namespace TreasureHunt.Commands;

internal static class LocateCommand
{
    public static GameState Locate(GameState state) =>
        state with
        {
            LatestMove =
            [
                GetCarriedTreasure(state.Carrying),
                Constants.Commands.RoomContentsLabel,
                ..
                state.TreasureLocations
                        .Where(l => l.Room != Constants.NoRoom)
                        .Select(l => $"  {l.Room}: {GameData.GetTreasureById(l.TreasureId).Name}")
                        .Append(string.Empty).ToArray()

            ]
        };

    private static string GetCarriedTreasure(int carrying) =>
        Constants.Commands.TreasureCarriedLabel(
            carrying != Constants.NoTreasure
                            ? GameData.GetTreasureById(carrying).Name
                            : Constants.Commands.TreasureNothing);
}
