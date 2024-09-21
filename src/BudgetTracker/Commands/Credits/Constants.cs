using BudgetTracker.Entities;

namespace BudgetTracker.Commands.Credits;

internal static class Constants
{
    public static class List
    {
        public const string CreditsListHeader = "List of Credits";
        public const string NoExpensesMessage = "No credit records have been created... please add some.";
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
        public const string Header = "Add Credit Item";
        public const string NameLabel = "Enter the credit title:";
        public const string DateLabel = "Enter date of the credit";
        public const string AmountLabel = "Enter the credit amount:";

        public static string[] SuccessMessage(Credit credit) =>
        [
            string.Empty,
            $"[green]Success:[/] The new credit item '{credit.Name}' was created with id={credit.Id}."
        ];
    }

    public static class Delete
    {
        public const string Header = "Delete Credit Item";
        public const string IdLabel = "Enter the credit id:";

        public static string[] SuccessMessage(Credit credit) =>
        [
            string.Empty,
            $"[green]Success:[/] The credit item '{credit.Name}' with id={credit.Id} was deleted."
        ];
    }

    public static class Edit
    {
        public const string Header = "Edit Credit Item";
        public const string IdLabel = "Enter credit id to edit:";
        public const string NameLabel = "Enter credit's new name";
        public const string DateLabel = "Enter new deposit date of the credit";
        public const string AmountLabel = "Enter the credit new amount";

        public static string[] GetSuccessMessage(Credit credit) =>
        [
            string.Empty,
            $"[green]Success:[/] The credit item with id={credit.Id} was retrieved for editing."
        ];

        public static string[] SuccessMessage(Credit credit) =>
        [
            string.Empty,
            $"[green]Success:[/] The credit item '{credit.Name}' with id={credit.Id} was updated."
        ];
    }
}
