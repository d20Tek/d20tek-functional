using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class AddWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Map(s => new WealthDataEntry(0, state.Console.GetName(), state.Console.GetCategories())
                .Map(entry => s.Repository.Create(entry))
                .Apply(result => s.Console.MarkupLine(GetDisplayResult(result)))
                .Map(_ => s with { Command = metadata.Name }));

    private static string GetName(this IAnsiConsole console) =>
        console.Prompt<string>(new TextPrompt<string>(Constants.Add.NameLabel));

    private static string GetCategory(this IAnsiConsole console) =>
        console.Prompt<string>(new TextPrompt<string>(Constants.Add.CategoryLabel).AllowEmpty());

    private static string[] GetCategories(this IAnsiConsole console) =>
        Enumerable.Empty<string>()
            .IterateUntil(
                categories => categories.Append(console.GetCategory()),
                categories => categories.Any() && categories.Last().IsEmpty())
            .Where(x => x.HasText())
            .ToArray();

    private static string GetDisplayResult(Maybe<WealthDataEntry> result) => Constants.Add.GetResultMessage(result);
}
