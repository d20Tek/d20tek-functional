using BudgetTracker.Entities;

namespace BudgetTracker.Commands.Expenses;

internal static class Constants
{
    public const string InvalidCatIdError = "[red]That category id does not exist.[/]";

    public static class List
    {
        public const string ExpensesListHeader = "List of Expenses";
        public const string NoExpensesMessage = "No expense records have been created... please add some.";
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 30;
        public const string ColumnCategory = "Category";
        public const int ColumnCategoryLen = 15;
        public const string ColumnCommittedDate = "Committed";
        public const int ColumnCommittedDateLen = 10;
        public const string ColumnActual = "Actual";
        public const int ColumnActualLen = 10;
        public const string CategoryNameError = "[red]Missing category[/]";
    }

    public static class Add
    {
        public const string Header = "Add Expense Item";
        public const string NameLabel = "Enter the expense title:";
        public const string CategoryIdLabel = "Enter expense category id:";
        public const string DateLabel = "Enter date of the expense";
        public const string ActualLabel = "Enter the expense actual amount:";

        public static string[] SuccessMessage(Expense expense) =>
        [
            string.Empty,
            $"[green]Success:[/] The new expense item '{expense.Name}' was created with id={expense.Id}."
        ];
    }

    public static class Delete
    {
        public const string Header = "Delete Expense Item";
        public const string IdLabel = "Enter the expense id:";

        public static string[] SuccessMessage(Expense expense) =>
        [
            string.Empty,
            $"[green]Success:[/] The expense item '{expense.Name}' with id={expense.Id} was deleted."
        ];
    }

    public static class Edit
    {
        public const string Header = "Edit Expense Item";
        public const string IdLabel = "Enter expense id to edit:";
        public const string NameLabel = "Enter expense's new name";
        public const string CategoryIdLabel = "Enter the expense's new category id";
        public const string DateLabel = "Enter new date of the expense";
        public const string ActualLabel = "Enter the expense new actual amount";

        public static string[] GetSuccessMessage(Expense expense) =>
        [
            string.Empty,
            $"[green]Success:[/] The expense item with id={expense.Id} was retrieved for editing."
        ];

        public static string[] SuccessMessage(Expense expense) =>
        [
            string.Empty,
            $"[green]Success:[/] The expense item '{expense.Name}' with id={expense.Id} was updated."
        ];
    }
}
