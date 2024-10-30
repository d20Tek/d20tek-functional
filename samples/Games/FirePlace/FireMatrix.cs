using D20Tek.Functional;

namespace FirePlace;

internal static class FireMatrix
{
    public static int[,] Initialize(FireConfig config, Game.RndFunc rnd) =>
        new int[config.Height, config.Width].ToIdentity()
            .Iter(matrix =>
                Enumerable.Range(0, config.Width)
                          .ForEach(x => matrix[config.BottomRow, x] = rnd(config.EmberStart, config.MaxHeat)));

    public static int[,] PropagateFire(this int[,] matrix, FireConfig config, Game.RndFunc rnd)
    {
        for (int y = config.Height - 2; y >= 0; y--)
        {
            Enumerable.Range(0, config.Width)
                      .ForEach(x =>
                          CalculateSpread(x, rnd(-1, 2), config.Width)
                            .Pipe(xOffset =>
                                matrix[y, xOffset] = CalcNewHeat(CalculateDecay(rnd), matrix.CalculateBelow(x, y))));
        }

        return matrix;
    }

    private static int CalculateDecay(Game.RndFunc rnd) => rnd(0, 2);

    private static int CalculateBelow(this int[,] matrix, int x, int y) => matrix[y + 1, x];

    private static int CalcNewHeat(int decay, int below) => Math.Max(below - decay, 0);

    private static int CalculateSpread(int x, int offset, int width) => Math.Clamp(x + offset, 0, width - 1);
}
