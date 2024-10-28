using D20Tek.Functional;
using WealthTracker.Features.Accounts;

namespace WealthTracker.Features;

internal static class Constants
{
    public static class Accounts
    {
        public const string ListUrl = "/account";
        public const string AddUrl = "/account/add";
        public static string EditUrl(int id) => $"/account/edit/{id}";
        public static string DeleteUrl(int id) => $"/account/delete/{id}";

        public const string MissingAccountError = "Error: cannot edit an account that doesn't exist";
        public static Result<T> FutureDateError<T>() where T : notnull =>
            Result<T>.Failure(
                Error.Validation("Record.DateOutOfRange", "You can only record amounts for past or current dates."));
    }

    public static class Reports
    {
        public const string CurrentUrl = "/report/current";
        public static string RecordUrl(int id) => $"/account/record/{id}";
        public static string UnrecordUrl(int id) => $"/account/unrecord/{id}";
    }
}
