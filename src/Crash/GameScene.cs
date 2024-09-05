using D20Tek.Minimal.Functional;
using Games.Common;

namespace Crash;

internal static class GameScene
{
    public static char[,] Create() => CreateSceneAsArray(CalcRoadEdges()).To2DArray();

    public static UpdateResponse Update(char[,] scene, int previousRoadUpdate, Game.RndFunc rnd) =>
        ShiftNewScene(scene)
            .Map(shiftedScene => CalculateRoadUpdate(scene, previousRoadUpdate, rnd)
            .Map(roadUpdate => AppendFinalRow(shiftedScene, roadUpdate)
            .Map(newScene => new UpdateResponse(newScene, roadUpdate, 0))));

    public static bool ShouldContinueRunning(char[,] scene, int newCarPosition) =>
        !(newCarPosition < 0 ||
          newCarPosition >= Constants.Width ||
          scene[1, newCarPosition] is not Constants.Scene.EmptySpace);

    private static char[][] CreateSceneAsArray((int Left, int Right) edges) =>
        Enumerable.Range(0, Constants.Height)
            .Select(i => Enumerable.Range(0, Constants.Width)
                .Select(j => (j < edges.Left || j > edges.Right)
                                ? Constants.Scene.FilledSpace
                                : Constants.Scene.EmptySpace)
                .ToArray())
            .ToArray();

    private static (int Left, int Right) CalcRoadEdges() =>
        ((Constants.Width - Constants.Scene.RoadWidth) / 2)
            .Map(leftEdge => (leftEdge, leftEdge + Constants.Scene.RoadWidth + 1));

    private static char[,] ShiftNewScene(char[,] scene) =>
        Enumerable.Range(0, Constants.Height - 1)
            .SelectMany(i => Enumerable.Range(0, Constants.Width)
                .Select(j => (i, j, value: scene[i + 1, j])))
            .Aggregate(scene, (acc, item) =>
            {
                acc[item.i, item.j] = item.value;
                return acc;
            });

    private static int CalculateRoadUpdate(char[,] scene, int previousRoadUpdate, Game.RndFunc rnd) =>
        rnd.RandomizeRoadUpdate(previousRoadUpdate)
            .Map(update => (update == Constants.Scene.ShiftLeft && 
                            scene[Constants.Height - 1, 0] == Constants.Scene.EmptySpace)
                        ? Constants.Scene.ShiftRight
                        : update)
            .Map(update => (update == Constants.Scene.ShiftRight &&
                            scene[Constants.Height - 1, Constants.Width - 1] == Constants.Scene.EmptySpace)
                        ? Constants.Scene.ShiftLeft
                        : update);

    private static char[,] AppendFinalRow(char[,] scene, int roadUpdate) =>
        roadUpdate switch
        {
            Constants.Scene.ShiftLeft =>
                Enumerable.Range(0, Constants.Width - 1)
                    .Select(j => (j, value: scene[Constants.Height - 1, j + 1]))
                    .Aggregate(scene, (acc, item) =>
                    {
                        acc[Constants.Height - 1, item.j] = item.value;
                        acc[Constants.Height - 1, Constants.Width - 1] = Constants.Scene.FilledSpace;
                        return acc;
                    }),
            Constants.Scene.ShiftRight =>
                Enumerable.Range(1, Constants.Width - 1)
                    .Reverse()
                    .Select(j => (j, value: scene[Constants.Height - 1, j - 1]))
                    .Aggregate(scene, (acc, item) =>
                    {
                        acc[Constants.Height - 1, item.j] = item.value;
                        acc[Constants.Height - 1, 0] = Constants.Scene.FilledSpace;
                        return acc;
                    }),
            _ => scene
        };
}
