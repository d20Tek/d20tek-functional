using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;

namespace BudgetTracker.Commands.Show;

internal static class ShowCurrentCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        ReconciledBuilder.GenerateSnapshot(
            new(Constants.Tables.DefaultStartDate, DateTimeOffset.Now),
            state.IncomeRepo,
            state.CategoryRepo,
            state.ExpenseRepo).ToIdentity()
                 .Iter(r => state.Console.DisplayHeader(Constants.Current.HeaderLabel))
                 .Iter(r => state.Console.Write(SnapshotTableHelper.CreateIncomeTable(r)))
                 .Iter(r => state.Console.Write(SnapshotTableHelper.CreateExpenseTable(r)))
                 .Map(r => state with { Command = metadata.Name });
}
