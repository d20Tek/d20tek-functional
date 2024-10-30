using D20Tek.Functional;

namespace FirePlace;

internal static class FireMatrix
{
    public static int[,] Initialize(Game.RndFunc rnd) =>
        new int[Constants.Height, Constants.Width].ToIdentity()
            .Iter(matrix =>
                Enumerable.Range(0, Constants.Width)
                          .ForEach(x => matrix[Constants.Height - 1, x] = rnd(Constants.EmberStart, Constants.MaxHeat)));

    public static int[,] PropagateFire(this int[,] matrix, Game.RndFunc rnd)
    {
        for (int y = Constants.Height - 2; y >= 0; y--)
        {
            for (int x = 0; x < Constants.Width; x++)
            {
                CalculateSpread(x, rnd(-1, 2))
                    .Pipe(xOffset => 
                        matrix[y, xOffset] = CalculateNewHeatValue(CalculateDecay(rnd), matrix.CalculateBelow(x, y)));
            }
        }

        return matrix;
    }

    private static int CalculateDecay(Game.RndFunc rnd) => rnd(0, 2);

    private static int CalculateBelow(this int[,] matrix, int x, int y) => matrix[y + 1, x];

    private static int CalculateNewHeatValue(int decay, int below) => Math.Max(below - decay, 0);

    private static int CalculateSpread(int x, int offset) => Math.Clamp(x + offset, 0, Constants.Width - 1);
}
