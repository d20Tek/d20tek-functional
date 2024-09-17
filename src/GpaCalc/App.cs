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
                   .Map(courses => courses.CalculateGpa(GradesTable.GradeToPoint))
                   .Map(console.DisplayGpaResults),
            onException: console.HandleErrorMessage);

    // todo: output a table at the end with all of the courses and total gpa.

    private static Course[] GetCourses(this IAnsiConsole console) =>
        CoursesExtensions.InitializeCourses().IterateUntil(
            c => c.Append(console.CreateCourse()),
            c => c.Any() && console.ConfirmGradesComplete())
            .ToArray();

    private static Course CreateCourse(this IAnsiConsole console) =>
        new(console.GetCourseName(), console.GetNumCredits(), console.GetLetterGrade());

    private static string GetCourseName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.CourseNameLabel));

    private static int GetNumCredits(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.CourseCreditsLabel)
            .Validate(v => v is > 0 and <= Constants.CreditsMaximum, Constants.CourseCreditsError));

    private static string GetLetterGrade(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.CourseGradeLabel, StringComparer.CurrentCultureIgnoreCase)
            .AddChoices(GradesTable.GetLetterGrades())
            .ShowChoices(false));

    private static bool ConfirmGradesComplete(this IAnsiConsole console) =>
        console.Confirm(Constants.GradeEntryCompleteLabel, false);

    private static int DisplayGpaResults(this IAnsiConsole console, double gpa) =>
        gpa.Apply(gpa => console.WriteMessage(Constants.GpaMessage(gpa)))
           .Map(_ => 0);

    private static int HandleErrorMessage(this IAnsiConsole console, Exception ex) =>
        console.Apply(c => c.MarkupLine(Constants.ExceptionErrorMessage(ex)))
               .Map(_ => -1);
}
