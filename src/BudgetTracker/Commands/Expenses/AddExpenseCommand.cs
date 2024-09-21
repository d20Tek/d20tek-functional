﻿using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Expenses;

internal static class AddExpenseCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Map(s => s.Console.GatherExpenseData(s.CategoryRepo)
                .Map(entry => s.ExpenseRepo.Create(entry))
                .Apply(result => s.Console.DisplayMaybe(result, Constants.Add.SuccessMessage))
                .Map(_ => s with { Command = metadata.Name }));

    private static Expense GatherExpenseData(this IAnsiConsole console, ICategoryRepository catRepo) =>
        new(0, console.GetName(), console.GetCategoryId(catRepo), console.GetCommittedDate(), console.GetActual());

    private static string GetName(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<string>(Constants.Add.NameLabel));

    private static int GetCategoryId(this IAnsiConsole console, ICategoryRepository catRepo) =>
        console.GetExistingCategoryId(Constants.Add.CategoryIdLabel, catRepo);

    private static DateTimeOffset GetCommittedDate(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetDate(console, Constants.Add.DateLabel, DateTimeOffset.Now);

    private static decimal GetActual(this IAnsiConsole console) =>
        CurrencyComponent.Input(console, Constants.Add.ActualLabel, false);
}

