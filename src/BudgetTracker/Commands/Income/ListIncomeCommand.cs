using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Incomes;

internal static class ListIncomeCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteMessage(Constants.List.IncomeListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.IncomeRepo.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(Income[] income) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnDepositDate).Width(Constants.List.ColumnDepositDateLen),
                new TableColumn(Constants.List.ColumnAmount).RightAligned().Width(Constants.List.ColumnAmountLen))
            .Apply(t => t.AddRowsForEntries(income));

    private static void AddRowsForEntries(this Table table, Income[] income) =>
        income
            .Map(e => (e.Length != 0)
                ? income.Select(entry => CreateRow(entry)).ToList()
                : [[string.Empty, Constants.List.NoExpensesMessage, string.Empty, string.Empty]])
            .Apply(rows => rows.ForEach(x => table.AddRow(x)));

    private static string[] CreateRow(Income income) =>
    [
        income.Id.ToString(),
        income.Name.CapOverflow(Constants.List.ColumnNameLen),
        income.DepositDate.ToDateString(),
        CurrencyComponent.Render(income.Amount)
    ];
}
