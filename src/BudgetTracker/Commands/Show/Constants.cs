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
            [string.Empty, $"The following income and actuals will be reconciled for {month}:"];
        public const string PerformReconcileLabel = "Are you sure you want to perform this reconciliation?\r\n" +
            "All of the individual income and expense entries will be collapsed into this snapshot.\r\n" +
            "And it cannot be undone.";
        public const string CancelledMessage = "This reconcile operation was cancelled.";
    }
}
