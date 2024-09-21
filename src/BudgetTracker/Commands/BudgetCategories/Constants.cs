using BudgetTracker.Entities;

namespace BudgetTracker.Commands.BudgetCategories;

internal static class Constants
{
    public static class List
    {
        public const string CategoryListHeader = "List of Budget Categories";
        public const string NoCategoriesMessage = "No budget categories are defined... please add some.";
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 35;
        public const string ColumnBudgetedAmount = "Budgeted Amount";
        public const int ColumnBudgetedAmountLen = 15;
    }

    public static class Add
    {
        public const string Header = "Add Budget Category";
        public const string NameLabel = "Enter the category name:";
        public const string AmountLabel = "Enter the category budget amount:";

        public static string[] SuccessMessage(BudgetCategory cat) =>
        [
            string.Empty,
            $"[green]Success:[/] The new budget category '{cat.Name}' was created with id={cat.Id}."
        ];
    }

    public static class Delete
    {
        public const string Header = "Delete Budget Category";
        public const string IdLabel = "Enter the category id:";

        public static string[] SuccessMessage(BudgetCategory cat) =>
        [
            string.Empty,
            $"[green]Success:[/] The category '{cat.Name}' with id={cat.Id} was deleted."
        ];
    }

    public static class Edit
    {
        public const string Header = "Edit Budget Category";
        public const string IdLabel = "Enter category id to edit:";
        public const string NameLabel = "Enter category's new name";
        public const string AmountLabel = "Enter the category's new budget amount";

        public static string[] GetSuccessMessage(BudgetCategory cat) =>
        [
            string.Empty,
            $"[green]Success:[/] The budget category with id={cat.Id} was retrieved for editing."
        ];

        public static string[] SuccessMessage(BudgetCategory cat) =>
        [
            string.Empty,
            $"[green]Success:[/] The budget category '{cat.Name}' with id={cat.Id} was updated."
        ];
    }
}
