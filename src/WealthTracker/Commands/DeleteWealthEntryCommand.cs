using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class DeleteWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Delete.Header))
             .Iter(s => s.Repository
                            .Delete(s.Console.GetId())
                            .Pipe(result => s.Console.DisplayResult(result, e => Constants.Delete.SuccessMessage(e))))
             .Map(_ => state with { Command = metadata.Name });

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Delete.IdLabel));
}
