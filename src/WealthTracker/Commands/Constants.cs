using D20Tek.Minimal.Functional;

namespace WealthTracker.Commands;

internal static class Constants
{
    public const string JoinSeparator = ", ";

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

        public static string GetResultMessage(Maybe<WealthDataEntry> result) =>
            result switch
            {
                Something<WealthDataEntry> s => $"[green]Success:[/] The new account '{s.Value.Name}' was created.",
                Error<WealthDataEntry> e => $"[red]Error:[/] {e.ErrorMessage.Message}",
                _ => "[red]Error:[/] Unexpected error occurred."
            };
    }
}
