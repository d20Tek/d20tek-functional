using D20Tek.Functional;

namespace MatrixDrop;

internal static class Matrix
{
    public static List<List<MatrixCell>> Initialize(int width,  int height, Random rnd) =>
        [.. Enumerable.Range(0, height).Select(_ => CreateRandomRow(width, rnd))];

    public static List<List<MatrixCell>> AddTopRow(this List<List<MatrixCell>> matrix, int width, Random rnd) =>
        matrix.ToIdentity()
              .Iter(m => m.Insert(0, CreateRandomRow(width, rnd)));

    public static List<List<MatrixCell>> RemoveBottomRow(this List<List<MatrixCell>> matrix, int height) =>
        matrix.ToIdentity()
              .Iter(m => (matrix.Count > height).IfTrueOrElse(() => matrix.RemoveAt(height - 1)));

    private static List<MatrixCell> CreateRandomRow(int width, Random rnd) =>
        [.. Enumerable.Range(0, width)
                      .Select(x => new MatrixCell
                      {
                          Char = Constants.Chars[rnd.Next(Constants.Chars.Length)],
                          Color = rnd.Next(1, 20) > 1 ? Constants.Colors.DimGreen : Constants.Colors.BrightGreen
                      })
        ];
}
