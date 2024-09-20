using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class DeleteCategoryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Delete.Header))
             .Map(s => s.Console.GetId()
                .Map(id => s.CategoryRepo.Delete(id))
                .Apply(result => s.Console.DisplayMaybe(result, Constants.Delete.SuccessMessage))
                .Map(_ => s with { Command = metadata.Name }));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Delete.IdLabel));
}
