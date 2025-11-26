using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Expenses;

internal static class ListExpenseCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteMessage(Constants.List.ExpensesListHeader))
             .Iter(s => s.Console.Write(CreateTable(s.ExpenseRepo.GetEntities(), s.CategoryRepo)))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(Expense[] expenses, ICategoryRepository catRepo) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnCategory).Width(Constants.List.ColumnCategoryLen),
                new TableColumn(Constants.List.ColumnCommittedDate).Width(Constants.List.ColumnCommittedDateLen),
                new TableColumn(Constants.List.ColumnActual).RightAligned().Width(Constants.List.ColumnActualLen))
            .ToIdentity()
            .Iter(t => t.AddRowsForEntries(expenses, catRepo));

    private static void AddRowsForEntries(this Table table, Expense[] expenses, ICategoryRepository catRepo) =>
        expenses.ToIdentity()
                .Map(e => (e.Length != 0)
                     ? expenses.Select(entry => CreateRow(entry, catRepo)).ToList()
                     : [[string.Empty, Constants.List.NoExpensesMessage, string.Empty, string.Empty, string.Empty]])
                .Iter(rows => rows.ForEach(x => table.AddRow(x)));

    private static string[] CreateRow(Expense expense, ICategoryRepository catRepo) =>
    [
        expense.Id.ToString(),
        expense.Name.CapOverflow(Constants.List.ColumnNameLen),
        catRepo.FindCategoryName(expense.CategoryId).CapOverflow(Constants.List.ColumnCategoryLen),
        expense.CommittedDate.ToDateString(),
        CurrencyComponent.Render(expense.Actual)
    ];

    private static string FindCategoryName(this ICategoryRepository catRepo, int catId) =>
        catRepo.GetEntityById(catId) switch
        {
            Success<BudgetCategory> s => s.GetValue().Name,
            _ => Constants.List.CategoryNameError
        };
}
