using D20Tek.Functional;
using Spectre.Console;

namespace TipCalc;

internal static class ShowTipResponseCommand
{
    public static void Handle(IAnsiConsole console, TipRequest request, TipResponse response) =>
        CreateTableForReponse(request, response)
            .Iter(_ => console.WriteLine())
            .Iter(console.Write);

    private static Identity<Table> CreateTableForReponse(TipRequest request, TipResponse response) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(string.Empty).Width(Constants.ColumnNameLen),
                new TableColumn(string.Empty).RightAligned().Width(Constants.ColumnAmountsLen))
            .HideHeaders()
            .ToIdentity()
            .Iter(table => table.AddRowsFor(request, response));

    private static Table AddRowsFor(this Table table, TipRequest request, TipResponse response) =>
        table.AddRow(Constants.PriceRowLabel, request.OriginalPrice.CurrencyDisplay())
             .AddRow(Constants.TipPercentRowLabel, request.TipPercentage.PercentageDisplay())
             .AddRow(Constants.TipAmountRowLabel, response.TipAmount.CurrencyDisplay())
             .AddRow(Constants.TotalAmountRowLabel, response.TotalAmount.CurrencyDisplay())
             .AddRowConditional(request.HasMultipleTippers, response.AmountPerTipper);

    private static Table AddRowConditional(this Table table, Func<bool> condition, decimal amountPerTipper) =>
        table.ToIdentity().Iter(t =>
        {
            if (condition() is true)
                t.AddRow(Constants.AmountPerTipperLabel, amountPerTipper.CurrencyDisplay());
        });
}
