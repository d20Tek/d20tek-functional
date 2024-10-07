namespace GpaCalc;

internal static class GradesTable
{
    private static readonly Dictionary<string, double> _gradePoints = new()
    {
        { "A+", 4.3 },
        { "A", 4.0 },
        { "A-", 3.7 },
        { "B+", 3.3 },
        { "B", 3.0 },
        { "B-", 2.7 },
        { "C+", 2.3 },
        { "C", 2.0 },
        { "C-", 1.7 },
        { "D+", 1.3 },
        { "D", 1.0 },
        { "D-", 0.7 },
        { "F", 0.0 }
    };

    public static string[] GetLetterGrades() => [.. _gradePoints.Keys];

    public static double GradeToPoint(string grade) =>
        _gradePoints.TryGetValue(grade, out double points) 
            ? points
            : throw new InvalidOperationException(Constants.InvalidGradeError(grade));
}
