﻿using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Credits;

internal static class AddCreditCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Map(s => s.Console.GatherCreditData()
                .Map(entry => s.CreditRepo.Create(entry))
                .Apply(result => s.Console.DisplayMaybe(result, Constants.Add.SuccessMessage))
                .Map(_ => s with { Command = metadata.Name }));

    private static Credit GatherCreditData(this IAnsiConsole console) =>
        new(0, console.GetName(), console.GetDepositDate(), console.GetAmount());

    private static string GetName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.Add.NameLabel));

    private static DateTimeOffset GetDepositDate(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Add.DateLabel, DateTimeOffset.Now);

    private static decimal GetAmount(this IAnsiConsole console) =>
        CurrencyComponent.Input(console, Constants.Add.AmountLabel, false);
}
