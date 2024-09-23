using Apps.Common;
using BudgetTracker.Entities;
using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ShowCurrentCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.Write(CreateIncomeTable(s.IncomeRepo.GetEntities())))
             .Apply(s => s.Console.Write(CreateExpenseTable(s.CategoryRepo.GetEntities(), s.ExpenseRepo)))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateIncomeTable(Income[] income) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Current.ColumnIncomeName).Width(Constants.Current.ColumnIncomeNameLen),
                new TableColumn(Constants.Current.ColumnIncomeAmount).RightAligned().Width(Constants.Current.ColumnIncomeAmountLen))
            .Apply(t => t.AddRowsForIncome(income));

    private static void AddRowsForIncome(this Table table, Income[] income) =>
        income
            .Map(e => (e.Length != 0)
                ? income.Select(entry => CreateIncomeRow(entry)).ToList()
                : [[Constants.Current.NoIncome, "0"]])
            .Apply(rows => rows.ForEach(x => table.AddRow(x)));

    private static string[] CreateIncomeRow(Income income) =>
    [
        income.Name.CapOverflow(Constants.Current.ColumnIncomeNameLen),
        CurrencyComponent.Render(income.Amount)
    ];

    private static Table CreateExpenseTable(BudgetCategory[] categories, IExpenseRepository expRepo) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Current.ColumnCategory).Width(Constants.Current.ColumnCategoryLen),
                new TableColumn(Constants.Current.ColumnBudget).RightAligned().Width(Constants.Current.ColumnBudgetLen),
                new TableColumn(Constants.Current.ColumnActual).RightAligned().Width(Constants.Current.ColumnActualLen),
                new TableColumn(Constants.Current.ColumnRemaining).RightAligned().Width(Constants.Current.ColumnRemainingLen))
            .Apply(t => t.AddRowsForEntries(categories, expRepo));

    private static void AddRowsForEntries(this Table table, BudgetCategory[] categories, IExpenseRepository expRepo) =>
        categories
            .Map(e => (e.Length != 0)
                ? categories.Select(cat => CreateRow(cat, expRepo.GetExpensesByCategory(cat.Id).Sum(x => x.Actual))).ToList()
                : [[Constants.Current.NoExpensesMessage, string.Empty, string.Empty, string.Empty]])
            .Apply(rows => rows.ForEach(x => table.AddRow(x)));

    private static string[] CreateRow(BudgetCategory category, decimal expensesSum) =>
    [
        category.Name.CapOverflow(Constants.Current.ColumnCategoryLen),
        CurrencyComponent.Render(category.BudgetedAmount),
        CurrencyComponent.Render(expensesSum),
        CurrencyComponent.Render(category.BudgetedAmount - expensesSum)
    ];
}
