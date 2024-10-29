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
        _trimmableDates = _dateRange.Take(3).ToArray();
        _accountRows = CalculateAccountRows(_repo.GetEntities(), _dateRange);
    }

    private string GetDateColumnCssClass(DateTimeOffset date) =>
        _trimmableDates.Contains(date) ? "text-center d-none d-md-table-cell" : "text-center";

    private static List<AccountRow> CalculateAccountRows(WealthDataEntity[] accounts, DateTimeOffset[] dateRange) =>
        accounts.Select(entry => CreateRow(entry, dateRange))
                .Concat(CreateTotalRow(accounts, dateRange))
                .ToList();

    private static AccountRow CreateRow(WealthDataEntity account, DateTimeOffset[] dateRange) =>
        new(
            account.Id.ToString(),
            account.Name,
            dateRange.Select(date => account.GetLatestValueFor(date)).ToArray(),
            account.GetLatestValue());

    private static AccountRow[] CreateTotalRow(WealthDataEntity[] accounts, DateTimeOffset[] dateRange) =>
    [
        new(
            string.Empty,
            Constants.Monthly.TotalLabel,
            dateRange.Select(d => accounts.Sum(x => x.GetLatestValueFor(d))).ToArray(),
            accounts.Sum(x => x.GetLatestValue()))
    ];

    private static DateTimeOffset[] GetDateRange() =>
        Enumerable.Range(0, Constants.Monthly.BackMonths)
            .Select(i => DateTimeOffset.Now.AddMonths(-(Constants.Monthly.BackMonths - 1) + i).StartOfMonth())
            .ToArray();
}
