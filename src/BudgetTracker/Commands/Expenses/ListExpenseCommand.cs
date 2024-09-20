using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Expenses;

internal static class ListExpenseCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteLine(Constants.List.ExpensesListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.ExpenseRepo.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(Expense[] expenses) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnCommittedDate).Width(Constants.List.ColumnCommittedDateLen),
                new TableColumn(Constants.List.ColumnActual).RightAligned().Width(Constants.List.ColumnActualLen))
            .Apply(t => t.AddRowsForEntries(expenses));

    private static void AddRowsForEntries(this Table table, Expense[] expenses) =>
        expenses
            .Map(e => (e.Length != 0)
                ? expenses.Select(entry => CreateRow(entry)).ToList()
                : [(string.Empty, Constants.List.NoExpensesMessage, string.Empty, string.Empty)])
            .Apply(rows => rows.ForEach(x => table.AddRow(x.Id, x.Name, x.Date, x.Actual)));

    private static (string Id, string Name, string Date, string Actual) CreateRow(Expense expense) =>
        (Id: expense.Id.ToString(),
         Name: expense.Name.CapOverflow(Constants.List.ColumnNameLen),
         Date: expense.CommittedDate.ToDateString(),
         Actual: $"{expense.Actual:C}");
}
