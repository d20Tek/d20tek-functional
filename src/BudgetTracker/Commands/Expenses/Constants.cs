namespace BudgetTracker.Commands.Expenses;

internal static class Constants
{
    public static class List
    {
        public const string ExpensesListHeader = "List of Expenses";
        public const string NoExpensesMessage = "No expense records have been created... please add some.";
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 30;
        public const string ColumnCommittedDate = "Committed";
        public const int ColumnCommittedDateLen = 10;
        public const string ColumnActual = "Actual";
        public const int ColumnActualLen = 10;
    }
}
