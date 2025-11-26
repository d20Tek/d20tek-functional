using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class ListCategoriesCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteMessage(Constants.List.CategoryListHeader))
             .Iter(s => s.Console.Write(CreateTable(s.CategoryRepo.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(BudgetCategory[] categories) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnBudgetedAmount).RightAligned().Width(Constants.List.ColumnBudgetedAmountLen))
            .ToIdentity()
            .Iter(t => t.AddRowsForEntries(categories));

    private static void AddRowsForEntries(this Table table, BudgetCategory[] categories) =>
        categories.ToIdentity()
                  .Map(e => (e.Length != 0)
                       ? categories.Select(entry => CreateRow(entry)).ToList()
                       : [(string.Empty, Constants.List.NoCategoriesMessage, string.Empty)])
                  .Iter(rows => rows.ForEach(x => table.AddRow(x.Id, x.Name, x.Amount)));

    private static (string Id, string Name, string Amount) CreateRow(BudgetCategory category) =>
        (Id: category.Id.ToString(),
         Name: category.Name.CapOverflow(Constants.List.ColumnNameLen),
         Amount: CurrencyComponent.Render(category.BudgetedAmount));
}
