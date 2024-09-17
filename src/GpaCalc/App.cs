using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace GpaCalc;

internal static class App
{
    public static int Run(IAnsiConsole console) =>
        console.Apply(c => c.DisplayAppHeader(Constants.AppTitle))
               .Map(c => c.GetCources())
               .Map(courses => courses.Fork(
                    x => courses.Sum(x => x.Credits * GradesTable.GradeToPoint(x.Grade)),
                    x => courses.Sum(x => x.Credits),
                    (totalPoints, totalCredits) => totalPoints / totalCredits))
               .Apply(gpa => console.WriteMessage(string.Empty, $"[green]Your GPA is:[/] {gpa:F2}"))
               .Map(_ => 0);

    // todo: wrap the Run method in a Try operation.
    // todo: move strings to Constants.
    // todo: output a table at the end with all of the courses and total gpa.

    private static Course[] GetCources(this IAnsiConsole console) =>
        InitializeCourses().IterateUntil(
            c => c.Append(new Course(
                    console.GetCourseName(),
                    console.GetNumCredits(),
                    console.GetLetterGrade())),
            c => c.Any() && console.ConfirmGradesComplete())
            .ToArray();

    private static IEnumerable<Course> InitializeCourses() => [];

    private static string GetCourseName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>("Enter the course name:"));

    private static int GetNumCredits(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>("Enter number of credits (1 - 5):")
            .Validate(v => v > 0 && v <= 5, "Error: credits must be between 1 and 5."));

    private static string GetLetterGrade(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>("Enter letter grade (A+ - F):")
            .AddChoices(GradesTable.GetLetterGrades())
            .ShowChoices(false));

    private static bool ConfirmGradesComplete(this IAnsiConsole console) =>
        console.Confirm("Are you done entering grades?", false);
}
