using D20Tek.Minimal.Functional;

namespace WealthTracker.Commands;

internal static class Constants
{
    public const string JoinSeparator = ", ";
    public const string ErrorLabel = "[red]Error:[/]";
    public const string UnexpectedErrorMesssage = $"{ErrorLabel} An unexpected error occurred.";

    public static class List
    {
        public const string AccountListHeader = "List of Accounts";
        public const string NoAccountsMessage = "No accounts are being tracked... please add some.";
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 50;
        public const string ColumnCategories = "Categories";
        public const int ColumnCategoriesLen = 40;
    }

    public static class Add
    {
        public const string Header = "Add a new account";
        public const string NameLabel = "Enter the account name:";
        public const string CategoryLabel = "Enter each category separately and press enter (or leave empty when done):";

        public static string SuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The new account '{s.Value.Name}' was created with id={s.Value.Id}.";
    }

    public static class Edit
    {
        public const string Header = "Edit an account";
        public const string IdLabel = "Enter account id to edit:";
        public const string NameLabel = "Enter account's new name:";
        public const string CategoryLabel = "Enter each category separately and press enter (or leave empty when done):";

        public static string ChangeCategoriesConfirm(string prevCategories) =>
            $"Do you wish to change the category list [[{prevCategories}]]?";

        public static string GetSuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The account with id={s.Value.Id} was retrieved for editing.";

        public static string SuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The account '{s.Value.Name}' with id={s.Value.Id} was updated.";
    }

    public static class Delete
    {
        public const string Header = "Delete an account";
        public const string IdLabel = "Enter the account id:";

        public static string SuccessMessage(Something<WealthDataEntry> s) => 
            $"[green]Success:[/] The account '{s.Value.Name}' with id={s.Value.Id} was deleted.";
    }

    public static class Record
    {
        public const string Header = "Record Amount";
        public const string IdLabel = "Enter the account id:";
        public const string DateLabel = "Enter the date of record:";
        public const string AmountLabel = "Enter the current value:";

        public static string GetSuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The account with id={s.Value.Id} was retrieved to change.";

        public static string SuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The account '{s.Value.Name}' (id={s.Value.Id}) amount was recorded.";
    }

    public static class Unrecord
    {
        public const string Header = "Remove Recorded Amount";
        public const string IdLabel = "Enter the account id:";
        public const string DateLabel = "Enter the date of record:";

        public static string GetSuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The account with id={s.Value.Id} was retrieved to change.";

        public static string SuccessMessage(Something<WealthDataEntry> s) =>
            $"[green]Success:[/] The account '{s.Value.Name}' (id={s.Value.Id}) had an amount record removed.";
    }

    public static class Current
    {
        public const string ListHeader = "Current Net Worth";
        public const string NoAccountsMessage = "No accounts are being tracked... please add some.";
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 60;
        public const string ColumnValue = "Value";
        public const int ColumnValueLen = 20;
        public static readonly (string Id, string Name, string Value) TotalBorder = 
            ("─────", "────────────────────────────────────────────────────────────", "────────────────────");
        public const string TotalLabel = "Total:";
    }

    public static class Monthly
    {
        public const string ListHeader = "Monthly Net Worth";
        public const string NoAccountsMessage = "No accounts are being tracked... please add some.";
        public const int BackMonths = 5;
        public const string ColumnId = "Id";
        public const int ColumnIdLen = 5;
        public const string ColumnName = "Name";
        public const int ColumnNameLen = 30;
        public const string ColumnValue = "Curr";
        public const int ColumnValueLen = 10;
        public const string TotalLabel = "Total:";
        public static string[] TotalBorder =
        [
            "─────", "──────────────────────────────", "──────────", "──────────",
            "──────────", "──────────", "──────────", "──────────"
        ];
    }
}
