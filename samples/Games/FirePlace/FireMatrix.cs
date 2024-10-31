using D20Tek.Functional;

namespace FirePlace;

internal static class FireMatrix
{
    public static int[,] Initialize(FireConfig config, Game.RndFunc rnd) =>
        new int[config.Height, config.Width].ToIdentity()
            .Iter(matrix =>
                Enumerable.Range(0, config.Width)
                          .ForEach(x => matrix[config.BottomRow, x] = rnd(config.EmberStart, config.MaxHeat)));

    public static int[,] PropagateFire(this int[,] matrix, FireConfig config, Game.RndFunc rnd) =>
        matrix.ToIdentity()
              .Iter(m => Enumerable.Range(0, config.Height - 1)
                                   .ForEach(y => m.PropagateRow(y, config, rnd)))
              .Map(_ => matrix);

    private static void PropagateRow(this int[,] matrix, int row, FireConfig config, Game.RndFunc rnd) =>
        Enumerable.Range(0, config.Width)
            .ForEach(x =>
                CalculateSpread(x, rnd(-1, 2), config.Width)
                    .Pipe(xOffset =>
                        matrix[row, xOffset] = CalcNewHeat(CalculateDecay(rnd), matrix.CalculateBelow(x, row))));

    private static int CalculateDecay(Game.RndFunc rnd) => rnd(0, 2);

    private static int CalculateBelow(this int[,] matrix, int x, int y) => matrix[y + 1, x];

    private static int CalcNewHeat(int decay, int below) => Math.Max(below - decay, 0);

    private static int CalculateSpread(int x, int offset, int width) => Math.Clamp(x + offset, 0, width - 1);
}
