using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class SnapshotTableHelper
{
    public static Table CreateIncomeTable(ReconciledSnapshot snapshot) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Tables.ColumnIncomeName).Width(Constants.Tables.ColumnIncomeNameLen),
                new TableColumn(Constants.Tables.ColumnIncomeAmount)
                    .RightAligned()
                    .Width(Constants.Tables.ColumnIncomeAmountLen))
            .Apply(t => t.AddRowsForIncome(snapshot));

    private static void AddRowsForIncome(this Table table, ReconciledSnapshot snapshot) =>
        snapshot
            .Apply(s => s.Income.Iter(x => table.AddRow(x.Name, CurrencyComponent.Render(x.Amount))))
            .Apply(s => table.AddTotalIncomeRow(s.TotalIncome));

    private static void AddTotalIncomeRow(this Table table, ReconciledIncome total) =>
        table.AddRow(Constants.Tables.TotalIncomeSeparator)
             .AddRow(total.Name, CurrencyComponent.Render(total.Amount));

    public static Table CreateExpenseTable(ReconciledSnapshot snapshot) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Tables.ColumnCategory).Width(Constants.Tables.ColumnCategoryLen),
                new TableColumn(Constants.Tables.ColumnBudget)
                    .RightAligned()
                    .Width(Constants.Tables.ColumnBudgetLen),
                new TableColumn(Constants.Tables.ColumnActual)
                    .RightAligned()
                    .Width(Constants.Tables.ColumnActualLen),
                new TableColumn(Constants.Tables.ColumnRemaining)
                    .RightAligned()
                    .Width(Constants.Tables.ColumnRemainingLen))
            .Apply(t => t.AddRowsForExpenses(snapshot))
            .Apply(t => t.AddTotalExpensesRow(snapshot.TotalExpenses));

    private static void AddRowsForExpenses(this Table table, ReconciledSnapshot snapshot) =>
        snapshot.Apply(s => s.CategoryExpenses.Iter(x =>
            table.AddRow(
                x.Category,
                CurrencyComponent.Render(x.Budget),
                CurrencyComponent.Render(x.Actual),
                CurrencyComponent.Render(x.Remaining))));

    private static void AddTotalExpensesRow(this Table table, ReconciledExpenses total) =>
        table.AddRow(Constants.Tables.TotalExpenseSeparator)
             .AddRow(
                total.Category,
                CurrencyComponent.Render(total.Budget),
                CurrencyComponent.Render(total.Actual),
                CurrencyComponent.Render(total.Remaining));
}
