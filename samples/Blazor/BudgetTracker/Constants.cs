using BudgetTracker.Domain;
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

    public static class Expense
    {
        public const string ListUrl = "/expense";
        public const string AddUrl = "/expense/add";
        public static string EditUrl(int id) => $"/expense/edit/{id}";
        public static string DeleteUrl(int id) => $"/expense/delete/{id}";
        public const string MissingExpenseError = "Error: cannot edit an expense record that doesn't exist";
    }

    public static class Reconcile
    {
        public static string ReconcileSucceeded = "Your monthly reconciliation completed successfully!";

        public static Result<ReconciledSnapshot> SnapshotEmptyError =
            Result<ReconciledSnapshot>.Failure(Error.Validation(
                "Snapshot.Empty",
                "There are no income or expenses in the current date range. No action was performed."));

        public static Result<ReconciledSnapshot> SnapshotAlreadyExistsError =
            Result<ReconciledSnapshot>.Failure(Error.Validation(
                "Snapshot.AlreadyExists",
                "A snapshot already exists for that month. We cannot overwrite the one that exists."));
    }
}
