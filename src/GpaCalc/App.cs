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
                   .Map(courses => courses.CalculateGpa(GradesTable.GradeToPoint)
                       .Map(gpa => ShowGpaCommand.Handle(console, courses, gpa))),
            onException: console.HandleErrorMessage);

    private static Course[] GetCourses(this IAnsiConsole console) =>
        CoursesExtensions.InitializeCourses().IterateUntil(
            c => c.Append(new(console.GetCourseName(), console.GetNumCredits(), console.GetLetterGrade())),
            c => c.Any() && console.ConfirmGradesComplete())
            .ToArray();

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

    private static int HandleErrorMessage(this IAnsiConsole console, Exception ex) =>
        console.Apply(c => c.MarkupLine(Constants.ExceptionErrorMessage(ex)))
               .Map(_ => -1);
}
