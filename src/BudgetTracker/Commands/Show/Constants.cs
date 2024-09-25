using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Commands.Show;

internal static class Constants
{
    public static class Tables
    {
        public const string ColumnIncomeName = "Income";
        public const int ColumnIncomeNameLen = 55;
        public const string ColumnIncomeAmount = "Amount";
        public const int ColumnIncomeAmountLen = 17;
        public const string NoIncome = "None";
        public const string NoExpensesMessage = "None";
        public const string ColumnCategory = "Expenses";
        public const int ColumnCategoryLen = 30;
        public const string ColumnBudget = "Budget";
        public const int ColumnBudgetLen = 12;
        public const string ColumnActual = "Actual";
        public const int ColumnActualLen = 12;
        public const string ColumnRemaining = "Remaining";
        public const int ColumnRemainingLen = 12;

        public static readonly string[] TotalIncomeSeparator = 
            ["───────────────────────────────────────────────────────", "─────────────────"];
        public static readonly string[] TotalExpenseSeparator = 
            ["──────────────────────────────", "────────────", "────────────", "────────────"];

        public static readonly DateTimeOffset DefaultStartDate = new(2024, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
    }

    public static class Current
    {
        public const string HeaderLabel = "Current Unreconciled Entries";
    }

    public static class Reconcile
    {
        public const string HeaderLabel = "Reconcile Monthly Actuals";
        public const string DateLabel = "Enter the month start you want to reconcile";
        public static string[] ItemsToReconcile(string month) =>
            [string.Empty, $"The following income and actuals will be reconciled for the month of {month}:"];
        public const string PerformReconcileLabel = "Are you sure you want to perform this reconciliation?\r\n" +
            "All of the individual income and expense entries will be collapsed into this snapshot.\r\n" +
            "And this action cannot be undone.";
        public const string CancelledMessage = "This reconcile operation was cancelled.";

        public static string[] ReconcileSucceeded =
            [string.Empty, "Your montly reconciliation [green]completed successfully[/]!"];

        public static Maybe<ReconciledSnapshot> SnapshotEmptyError =
            new Failure<ReconciledSnapshot>(Error.Validation(
                "Snapshot.Empty",
                "There are no income or expenses in the current date range. No action was performed."));

        public static Maybe<ReconciledSnapshot> SnapshotAlreadyExistsError =
            new Failure<ReconciledSnapshot>(Error.Validation(
                "Snapshot.AlreadyExists",
                "A snapshot already exists for that month. We cannot overwrite the one that exists."));
    }
}
