using WealthTracker.Domain;

namespace WealthTracker.Features.Reports;

public partial class YearlyNetWorth
{
    internal record YearRow(string Year, decimal Value, decimal Delta);

    private List<YearRow> _years = [];

    protected override void OnInitialized()
    {
        var accounts = _repo.GetAll().Match(s => s.ToArray(), _ => []);
        _years = CalculateYearResults(accounts, GetDateRange());
    }

    private static List<YearRow> CalculateYearResults(WealthDataEntity[] accounts, DateTimeOffset[] dateRange) =>
        [.. dateRange.Aggregate(
            (PrevTotal: 0M, Rows: new List<YearRow>()),
            (acc, date) =>
                accounts.Sum(x => x.GetLatestValueFor(date)).ToIdentity()
                        .Iter(currentTotal => acc.Rows.Add(
                            new YearRow((date.Year - 1).ToString(), currentTotal, currentTotal - acc.PrevTotal)))
                        .Map(currentTotal => (currentTotal, acc.Rows)),
            acc => acc.Rows.Concat(CreateYtdRow(accounts.Sum(x => x.GetLatestValue()), acc.PrevTotal)))];

    private static YearRow[] CreateYtdRow(decimal ytdTotal, decimal prevTotal) =>
    [
        new (Constants.Yearly.YtdLabel, ytdTotal, ytdTotal - prevTotal)
    ];

    private static DateTimeOffset[] GetDateRange() =>
        [.. Enumerable.Range(0, Constants.Yearly.BackYears)
            .Select(i => DateTimeOffset.Now.AddYears(-(Constants.Yearly.BackYears - 1) + i).StartOfYear())];
}
