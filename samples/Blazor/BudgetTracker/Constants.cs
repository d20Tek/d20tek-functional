using D20Tek.Functional;

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
        public const string MissingCategoryError = "Error: cannot edit a budget category that doesn't exist";
    }

    public static class Income
    {
        public const string ListUrl = "/income";
        public const string AddUrl = "/income/add";
        public static string EditUrl(int id) => $"/income/edit/{id}";
        public static string DeleteUrl(int id) => $"/income/delete/{id}";
        public const string MissingIncomeError = "Error: cannot edit an income record that doesn't exist";
    }
}
