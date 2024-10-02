using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace GpaCalc;

internal static class ShowGpaCommand
{
    public static int Handle(IAnsiConsole console, Course[] courses, double gpa) =>
        CreateTable(courses, gpa)
            .Iter(_ => console.WriteMessage(Constants.TableTitle))
            .Iter(table => console.Write(table))
            .Map(_ => 0);

    private static Identity<Table> CreateTable(Course[] courses, double gpa) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.ColumnName).Width(Constants.ColumnNameLen),
                new TableColumn(Constants.ColumnCredits).Centered().Width(Constants.ColumnCreditsLen),
                new TableColumn(Constants.ColumnGrade).Width(Constants.ColumnGradeLen))
            .ToIdentity()
            .Iter(table => table.AddRowsFor(courses, gpa));

    private static void AddRowsFor(this Table table, Identity<Course[]> courses, double gpa) =>
        courses.Map(e => e.Select(course => CreateRow(course))
                          .Concat(CreateTotalRow(gpa, e.Sum(x => x.Credits)))
                          .ToList())
               .Iter(list => list.ForEach(x => table.AddRow(x.Name, x.Credits, x.Grade)));

    private static (string Name, string Credits, string Grade) CreateRow(Course course) =>
        (Name: course.Name.CapOverflow(Constants.ColumnNameLen),
         Credits: course.Credits.ToString(),
         Grade: $"  {course.Grade}");

    private static (string Name, string Credits, string Grade)[] CreateTotalRow(double gpa, int totalCredits) =>
    [
        Constants.OverallBorder,
        (Name: Constants.TotalOverallGpa,
         Credits: $"{totalCredits}",
         Grade: $" {gpa:F2}")
    ];
}
