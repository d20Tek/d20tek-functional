using Apps.Common;
using Apps.Repositories;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands;

internal static class CommonHandlers
{
    public static AppState ShowCommands(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteMessage(Constants.CommandListMessage))
             .Map(s => s with { Command = metadata.Name });

    public static AppState Exit(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.MarkupLine(Constants.ExitCommandMessage))
             .Map(s => s with { Command = metadata.Name, CanContinue = false });

    public static AppState Error(AppState state, string command, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.MarkupLine(Constants.ErrorCommandMessage(command)))
             .Map(s => s with { Command = metadata.Name });

    // todo: remove this code after migration
    public static AppState DeleteFromRepositoryOld<TEntity>(
        AppState state,
        CommandTypeMetadata metadata,
        string label,
        Func<int, D20Tek.Minimal.Functional.Maybe<TEntity>> deleteFunc) => state;

    public static AppState DeleteFromRepository<TEntity>(
        AppState state,
        CommandTypeMetadata metadata,
        string label,
        Func<int, Result<TEntity>> deleteFunc)
        where TEntity : IEntity =>
        state.Iter(s => s.Console.DisplayHeader(label))
             .Iter(s => deleteFunc(s.Console.Prompt(new TextPrompt<int>(Constants.DeleteIdLabel)))
                            .Pipe(result => s.Console.DisplayResult(result, x => Constants.DeleteSuccessMessage(x))))
             .Map(s => s with { Command = metadata.Name });
}
