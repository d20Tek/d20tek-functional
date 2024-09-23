using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Commands.Show;

internal static class ShowCurrentCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        ReconciledBuilder.GenerateSnapshot(
            Constants.Tables.DefaultStartDate,
            DateTimeOffset.Now,
            state.IncomeRepo,
            state.CategoryRepo,
            state.ExpenseRepo)
                .Apply(r => state.Console.DisplayHeader(Constants.Current.HeaderLabel))
                .Apply(r => state.Console.Write(SnapshotTableHelper.CreateIncomeTable(r)))
                .Apply(r => state.Console.Write(SnapshotTableHelper.CreateExpenseTable(r)))
                .Map(r => state with { Command = metadata.Name });
}
