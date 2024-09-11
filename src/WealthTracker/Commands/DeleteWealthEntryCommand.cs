﻿using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class DeleteWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Delete.Header))
             .Map(s => s.Console.GetId()
                .Map(id => s.Repository.Delete(id))
                .Apply(result => s.Console.Render(result, Constants.Delete.SuccessMessage))
                .Map(_ => s with { Command = metadata.Name }));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Delete.IdLabel));
}