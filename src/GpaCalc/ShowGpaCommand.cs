using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace GpaCalc;

internal static class ShowGpaCommand
{
    public static int Handle(IAnsiConsole console, Course[] courses, double gpa) =>
        CreateTable(courses, gpa)
            .Apply(_ => console.WriteMessage(Constants.TableTitle))
            .Apply(table => console.Write(table))
            .Map(_ => 0);

    private static Table CreateTable(Course[] courses, double gpa) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.ColumnName).Width(Constants.ColumnNameLen),
                new TableColumn(Constants.ColumnCredits).Centered().Width(Constants.ColumnCreditsLen),
                new TableColumn(Constants.ColumnGrade).Width(Constants.ColumnGradeLen))
            .Apply(table => table.AddRowsFor(courses, gpa));

    private static void AddRowsFor(this Table table, Course[] courses, double gpa) =>
        courses.Map(e => e.Select(course => CreateRow(course))
                   .Concat(CreateTotalRow(gpa))
                   .ToList())
               .ForEach(x => table.AddRow(x.Name, x.Credits, x.Grade));

    private static (string Name, string Credits, string Grade) CreateRow(Course course) =>
        (Name: course.Name.CapOverflow(Constants.ColumnNameLen),
         Credits: course.Credits.ToString(),
         Grade: $"  {course.Grade}");

    private static (string Name, string Credits, string Grade)[] CreateTotalRow(double gpa) =>
    [
        Constants.OverallBorder,
        (Name: Constants.TotalOverallGpa,
         Credits: string.Empty,
         Grade: $" {gpa:F2}")
    ];
}
