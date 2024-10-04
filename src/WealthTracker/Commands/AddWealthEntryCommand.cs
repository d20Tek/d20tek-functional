using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class AddWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Add.Header))
             .Map(s => s.Repository.Create(s.Console.CreateDataEntry())
                 .Iter(result => s.Console.DisplayResult<WealthDataEntry>(
                     result, e => Constants.Add.SuccessMessage(e))))
             .Map(_ => state with { Command = metadata.Name }).GetValue();

    private static Identity<WealthDataEntry> CreateDataEntry(this IAnsiConsole console) =>
        new WealthDataEntry(0, console.GetName(), console.GetCategories());

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
}
