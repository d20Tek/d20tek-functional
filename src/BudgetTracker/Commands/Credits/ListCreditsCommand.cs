using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Credits;

internal static class ListCreditsCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteMessage(Constants.List.CreditsListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.CreditRepo.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(Credit[] credits) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnDepositDate).Width(Constants.List.ColumnDepositDateLen),
                new TableColumn(Constants.List.ColumnAmount).RightAligned().Width(Constants.List.ColumnAmountLen))
            .Apply(t => t.AddRowsForEntries(credits));

    private static void AddRowsForEntries(this Table table, Credit[] credits) =>
        credits
            .Map(e => (e.Length != 0)
                ? credits.Select(entry => CreateRow(entry)).ToList()
                : [[string.Empty, Constants.List.NoExpensesMessage, string.Empty, string.Empty]])
            .Apply(rows => rows.ForEach(x => table.AddRow(x)));

    private static string[] CreateRow(Credit credit) =>
    [
        credit.Id.ToString(),
        credit.Name.CapOverflow(Constants.List.ColumnNameLen),
        credit.DepositDate.ToDateString(),
        CurrencyComponent.Render(credit.Amount)
    ];
}
