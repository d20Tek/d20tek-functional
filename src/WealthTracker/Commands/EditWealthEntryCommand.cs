using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class EditWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Edit.Header))
             .Map(s => s.Repository.GetEntityById(s.Console.GetId())
                 .Iter(result => s.Console.DisplayResult<WealthDataEntry>(result, e => Constants.Edit.GetSuccessMessage(e)))
                 .Iter(result => PerformEdit(s.Console, s.Repository, result))
                 .Map(_ => s with { Command = metadata.Name })).GetValue();

    private static void PerformEdit(
        IAnsiConsole console,
        IWealthRepository repo,
        Result<WealthDataEntry> editEntry) =>
            editEntry.Iter(v => v.UpdateEntry(console.GetName(v.Name), console.GetCategories(v.Categories)))
                     .Map(entry => repo.Update(entry))
                     .Iter(result => console.DisplayResult(result, e => Constants.Edit.SuccessMessage(e)));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Edit.IdLabel));

    private static string GetName(this IAnsiConsole console, string prevName) =>
        console.Prompt<string>(new TextPrompt<string>(Constants.Edit.NameLabel)
                                    .DefaultValue(prevName));

    private static string GetCategory(this IAnsiConsole console) =>
        console.Prompt<string>(new TextPrompt<string>(Constants.Edit.CategoryLabel).AllowEmpty());

    private static string[] GetCategories(this IAnsiConsole console, string[] prevCategories) =>
        console.Confirm(Constants.Edit.ChangeCategoriesConfirm(prevCategories.AsString()))
            ? Enumerable.Empty<string>()
                .IterateUntil(
                    categories => categories.Append(console.GetCategory()),
                    categories => categories.Any() && categories.Last().IsEmpty())
                .Where(x => x.HasText())
                .ToArray()
            : prevCategories;
}
