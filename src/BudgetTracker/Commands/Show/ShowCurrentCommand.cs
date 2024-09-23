using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ShowCurrentCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        ReconciledBuilder.GenerateSnapshot(
            Constants.Current.DefaultStartDate,
            DateTimeOffset.Now,
            state.IncomeRepo,
            state.CategoryRepo,
            state.ExpenseRepo)
             .Apply(r => state.Console.Write(CreateIncomeTable(r)))
             .Apply(r => state.Console.Write(CreateExpenseTable(r)))
             .Map(r => state with { Command = metadata.Name });

    private static Table CreateIncomeTable(ReconciledSnapshot snapshot) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Current.ColumnIncomeName).Width(Constants.Current.ColumnIncomeNameLen),
                new TableColumn(Constants.Current.ColumnIncomeAmount).RightAligned().Width(Constants.Current.ColumnIncomeAmountLen))
            .Apply(t => t.AddRowsForIncome(snapshot));

    private static void AddRowsForIncome(this Table table, ReconciledSnapshot snapshot) =>
        snapshot
            .Apply(s => s.Income.Iter(x => table.AddRow(x.Name, CurrencyComponent.Render(x.Amount))))
            .Apply(s => table.AddRow(s.TotalIncome.Name, CurrencyComponent.Render(s.TotalIncome.Amount)));

    private static Table CreateExpenseTable(ReconciledSnapshot snapshot) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Current.ColumnCategory).Width(Constants.Current.ColumnCategoryLen),
                new TableColumn(Constants.Current.ColumnBudget).RightAligned().Width(Constants.Current.ColumnBudgetLen),
                new TableColumn(Constants.Current.ColumnActual).RightAligned().Width(Constants.Current.ColumnActualLen),
                new TableColumn(Constants.Current.ColumnRemaining).RightAligned().Width(Constants.Current.ColumnRemainingLen))
            .Apply(t => t.AddRowsForExpenses(snapshot));

    private static void AddRowsForExpenses(this Table table, ReconciledSnapshot snapshot) =>
        snapshot.Apply(s => s.CategoryExpenses.Iter(x => 
            table.AddRow(
                x.Category,
                CurrencyComponent.Render(x.Budget),
                CurrencyComponent.Render(x.Actual),
                CurrencyComponent.Render(x.Remaining))));
}
