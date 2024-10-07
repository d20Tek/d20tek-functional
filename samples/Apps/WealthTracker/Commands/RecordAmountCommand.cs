﻿using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class RecordAmountCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Record.Header))
             .Iter(s => s.Repository
                            .GetEntityById(s.Console.GetId())
                            .Pipe(result => s.Console.DisplayResult(result, e => Constants.Delete.SuccessMessage(e)))
                            .Pipe(result => PerformChange(s.Console, s.Repository, result)))
             .Map(s => s with { Command = metadata.Name });

    private static void PerformChange(
        IAnsiConsole console,
        IWealthRepository repo,
        Result<WealthDataEntry> updateEntry) =>
            updateEntry.Iter(v => v.AddDailyValue(DateTimeOffset.Now, console.GetAmount()))
                       .Map(entry => repo.Update(entry))
                       .Iter(result => console.DisplayResult(result, e => Constants.Record.SuccessMessage(e)));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Record.IdLabel));

    private static decimal GetAmount(this IAnsiConsole console) =>
        CurrencyComponent.Input(console, Constants.Record.AmountLabel);
}