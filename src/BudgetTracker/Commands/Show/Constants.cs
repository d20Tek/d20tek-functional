namespace BudgetTracker.Commands.Show;

internal static class Constants
{
    public static class Current
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

    public static class Reconcile
    {
        public const string DateLabel = "Enter the month start you want to reconcile";
    }
}
