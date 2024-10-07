using BudgetTracker.Entities;

namespace BudgetTracker.Commands.Incomes;

internal static class Constants
{
    public static class List
    {
        public const string IncomeListHeader = "List of Income";
        public const string NoExpensesMessage = "No income records have been created... please add some.";
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 30;
        public const string ColumnDepositDate = "Deposited";
        public const int ColumnDepositDateLen = 10;
        public const string ColumnAmount = "Amount";
        public const int ColumnAmountLen = 10;
    }

    public static class Add
    {
        public const string Header = "Add Income Item";
        public const string NameLabel = "Enter the income title:";
        public const string DateLabel = "Enter date of the income";
        public const string AmountLabel = "Enter the income amount:";

        public static string[] SuccessMessage(Income income) =>
        [
            string.Empty,
            $"[green]Success:[/] The new income item '{income.Name}' was created with id={income.Id}."
        ];
    }

    public static class Delete
    {
        public const string Header = "Delete Income Item";
        public const string IdLabel = "Enter the income id:";

        public static string[] SuccessMessage(Income income) =>
        [
            string.Empty,
            $"[green]Success:[/] The income item '{income.Name}' with id={income.Id} was deleted."
        ];
    }

    public static class Edit
    {
        public const string Header = "Edit Income Item";
        public const string IdLabel = "Enter income id to edit:";
        public const string NameLabel = "Enter income's new name";
        public const string DateLabel = "Enter new deposit date of the income";
        public const string AmountLabel = "Enter the income new amount";

        public static string[] GetSuccessMessage(Income income) =>
        [
            string.Empty,
            $"[green]Success:[/] The income item with id={income.Id} was retrieved for editing."
        ];

        public static string[] SuccessMessage(Income income) =>
        [
            string.Empty,
            $"[green]Success:[/] The income item '{income.Name}' with id={income.Id} was updated."
        ];
    }
}
