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
    }
}
