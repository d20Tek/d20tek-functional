namespace MatrixDrop;

internal static class Matrix
{
    public static List<List<MatrixCell>> Initialize(int width,  int height, Random rnd)
    {
        var matrix = new List<List<MatrixCell>>();
        for (int y = 0; y < height; y++)
        {
            matrix.Add(CreateRandomRow(width, rnd));
        }

        return matrix;
    }

    public static List<List<MatrixCell>> AddTopRow(this List<List<MatrixCell>> matrix, int width, Random rnd)
    {
        matrix.Insert(0, CreateRandomRow(width, rnd));
        return matrix;
    }

    public static List<List<MatrixCell>> RemoveBottomRow(this List<List<MatrixCell>> matrix, int height)
    {
        if (matrix.Count >= height)
        {
            matrix.RemoveAt(height - 1);
        }

        return matrix;
    }

    private static List<MatrixCell> CreateRandomRow(int width, Random rnd) =>
        Enumerable.Range(0, width).Select(x => GetRandomChar(rnd)).ToList();

    private static MatrixCell GetRandomChar(Random rnd) => new()
    {
        Char = Constants.Chars[rnd.Next(Constants.Chars.Length)],
        Color = rnd.Next(1, 20) > 1 ? Constants.Colors.DimGreen : Constants.Colors.BrightGreen
    };
}
