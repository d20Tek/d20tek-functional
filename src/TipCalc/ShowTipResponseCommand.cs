﻿using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace TipCalc;

internal static class ShowTipResponseCommand
{
    public static void Handle(IAnsiConsole console, TipRequest request, TipResponse response) =>
        CreateTableForReponse(request, response)
            .Apply(_ => console.WriteLine())
            .Apply(table => console.Write(table));

    private static Table CreateTableForReponse(TipRequest request, TipResponse response) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(string.Empty).Width(Constants.ColumnNameLen),
                new TableColumn(string.Empty).RightAligned().Width(Constants.ColumnAmountsLen))
            .HideHeaders()
            .Apply(table => table.AddRowsFor(request, response));

    private static Table AddRowsFor(this Table table, TipRequest request, TipResponse response) =>
        table.AddRow(Constants.PriceRowLabel, request.OriginalPrice.CurrencyDisplay())
             .AddRow(Constants.TipPercentRowLabel, request.TipPercentage.PercentageDisplay())
             .AddRow(Constants.TipAmountRowLabel, response.TipAmount.CurrencyDisplay())
             .AddRow(Constants.TotalAmountRowLabel, response.TotalAmount.CurrencyDisplay())
             .AddRowConditional(request.HasMultipleTippers, response.AmountPerTipper);

    private static Table AddRowConditional(this Table table, Func<bool> condition, decimal amountPerTipper) =>
        table.Apply(t => condition().IfTrueOrElse(
            () => _ = t.AddRow(Constants.AmountPerTipperLabel, amountPerTipper.CurrencyDisplay())));
}
