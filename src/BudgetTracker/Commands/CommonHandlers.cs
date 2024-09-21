using Apps.Common;
using Apps.Repositories;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands;

internal static class CommonHandlers
{
    public static AppState ShowCommands(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteMessage(Constants.CommandListMessage))
             .Map(s => s with { Command = metadata.Name });

    public static AppState Exit(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.MarkupLine(Constants.ExitCommandMessage))
             .Map(s => s with { Command = metadata.Name, CanContinue = false });

    public static AppState Error(AppState state, string command, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.MarkupLine(Constants.ErrorCommandMessage(command)))
             .Map(s => s with { Command = metadata.Name });

    public static AppState DeleteFromRepository<TEntity>(
        AppState state,
        CommandTypeMetadata metadata,
        string label,
        Func<int, Maybe<TEntity>> deleteFunc)
        where TEntity : IEntity =>
        state.Apply(s => s.Console.DisplayHeader(label))
             .Map(s => s.Console.Prompt(new TextPrompt<int>(Constants.DeleteIdLabel))
                .Map(id => deleteFunc(id))
                .Apply(result => s.Console.DisplayMaybe(result, x => Constants.DeleteSuccessMessage(x)))
                .Map(_ => s with { Command = metadata.Name }));
}
