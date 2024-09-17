using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace GpaCalc;

internal static class App
{
    public static int Run(IAnsiConsole console) =>
        FunctionalExtensions.TryExcept<int>(
            operation: () => console.Apply(c => c.DisplayAppHeader(Constants.AppTitle))
                   .Map(c => c.GetCourses())
                   .Map(courses => courses.Fork(
                        x => courses.Sum(x => x.Credits * GradesTable.GradeToPoint(x.Grade)),
                        x => courses.Sum(x => x.Credits),
                        (totalPoints, totalCredits) => totalPoints / totalCredits))
                   .Apply(gpa => console.WriteMessage(Constants.GpaMessage(gpa)))
                   .Map(_ => 0),
            onException: ex => console.Apply(c => c.MarkupLine(Constants.ExceptionErrorMessage(ex)))
                   .Map(_ => -1));

    // todo: make input letter grades case insensitive.
    // todo: move InitializeCourses and Gpa calculation to CoursesExtensions class.
    // todo: output a table at the end with all of the courses and total gpa.

    private static Course[] GetCourses(this IAnsiConsole console) =>
        InitializeCourses().IterateUntil(
            c => c.Append(new Course(
                    console.GetCourseName(),
                    console.GetNumCredits(),
                    console.GetLetterGrade())),
            c => c.Any() && console.ConfirmGradesComplete())
            .ToArray();

    private static IEnumerable<Course> InitializeCourses() => [];

    private static string GetCourseName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.CourseNameLabel));

    private static int GetNumCredits(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.CourseCreditsLabel)
            .Validate(v => v is > 0 and <= Constants.CreditsMaximum, Constants.CourseCreditsError));

    private static string GetLetterGrade(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.CourseGradeLabel)
            .AddChoices(GradesTable.GetLetterGrades())
            .ShowChoices(false));

    private static bool ConfirmGradesComplete(this IAnsiConsole console) =>
        console.Confirm(Constants.GradeEntryCompleteLabel, false);
}
