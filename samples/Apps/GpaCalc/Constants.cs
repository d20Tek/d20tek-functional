namespace GpaCalc;

internal static class Constants
{
    public const string AppTitle = "GPA Calc";
    public const string CourseNameLabel = "Enter the course name:";
    public const string CourseCreditsLabel = "Enter number of credits (1 - 5):";
    public const string CourseCreditsError = "[red]Error: credits must be between 1 and 5.[/]";
    public const string CourseGradeLabel = "Enter letter grade (A+ - F):";
    public const string GradeEntryCompleteLabel = "Are you done entering grades?";
    public const int CreditsMaximum = 5;

    public const string ColumnName = "Course";
    public const int ColumnNameLen = 28;
    public const string ColumnCredits = "Credits";
    public const int ColumnCreditsLen = 7;
    public const string ColumnGrade = "Grade";
    public const int ColumnGradeLen = 6;
    public const string TotalOverallGpa = "Overall GPA:";

    public static readonly (string Id, string Name, string Value) OverallBorder =
        ("────────────────────────────", "───────", "──────");

    public static readonly string[] TableTitle = [string.Empty, " Grading Transcipt"];

    public static string InvalidGradeError(string grade) => $"'{grade}' was an invalid letter grade.";

    public static string ExceptionErrorMessage(Exception ex) => $"[red]Error:[/] {ex.Message}";
}
