using WealthTracker.Domain;

namespace WealthTracker.Features.Reports;

public partial class MonthlyNetWorth
{
    internal record AccountRow(string Id, string Name, decimal[] Values, decimal Current);

    private DateTimeOffset[] _dateRange = [];
    private DateTimeOffset[] _trimmableDates = [];
    private List<AccountRow> _accountRows = [];

    protected override void OnInitialized()
    {
        _dateRange = GetDateRange();
        _trimmableDates = [.. _dateRange.Take(3)];
        var accounts = _repo.GetAll().Match(s => s.ToArray(), _ => []);
        _accountRows = CalculateAccountRows(accounts, _dateRange);
    }

    private string GetDateColumnCssClass(DateTimeOffset date) =>
        _trimmableDates.Contains(date) ? "text-center d-none d-md-table-cell" : "text-center";

    private static List<AccountRow> CalculateAccountRows(WealthDataEntity[] accounts, DateTimeOffset[] dateRange) =>
        [.. accounts.Select(entry => CreateRow(entry, dateRange)), .. CreateTotalRow(accounts, dateRange)];

    private static AccountRow CreateRow(WealthDataEntity account, DateTimeOffset[] dateRange) =>
        new(
            account.Id.ToString(),
            account.Name,
            [.. dateRange.Select(account.GetLatestValueFor)],
            account.GetLatestValue());

    private static AccountRow[] CreateTotalRow(WealthDataEntity[] accounts, DateTimeOffset[] dateRange) =>
    [
        new(
            string.Empty,
            Constants.Monthly.TotalLabel,
            [.. dateRange.Select(d => accounts.Sum(x => x.GetLatestValueFor(d)))],
            accounts.Sum(x => x.GetLatestValue()))
    ];

    private static DateTimeOffset[] GetDateRange() =>
        [.. Enumerable.Range(0, Constants.Monthly.BackMonths)
                      .Select(i => DateTimeOffset.Now.AddMonths(-(Constants.Monthly.BackMonths - 1) + i).StartOfMonth())];
}
