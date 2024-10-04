using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class DeleteWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Delete.Header))
             .Map(s => s.Repository.Delete(s.Console.GetId())
                .Iter(result => s.Console.DisplayResult<WealthDataEntry>(
                    result, e => Constants.Delete.SuccessMessage(e)))
                .Map(_ => s with { Command = metadata.Name })).GetValue();

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Delete.IdLabel));
}
