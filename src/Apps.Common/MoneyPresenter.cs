namespace Apps.Common;

internal static class MoneyPresenter
{
    public static string Render(decimal value, string monetarySymbol = "$") =>
        $"{monetarySymbol} {value:N0}";
}
