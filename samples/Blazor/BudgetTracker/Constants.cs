namespace BudgetTracker;

internal static class Constants
{
    public const string TotalIncomeLabel = "Total Income:";
    public const string TotalExpensesLabel = "Total Expenses:";

    public static class Categories
    {
        public const string ListUrl = "/budget-category";
        public const string AddUrl = "/budget-category/add";
        public static string EditUrl(int id) => $"/budget-category/edit/{id}";
        public static string DeleteUrl(int id) => $"/budget-category/delete/{id}";
    }
}
