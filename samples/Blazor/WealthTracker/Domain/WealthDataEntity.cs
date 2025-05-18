namespace WealthTracker.Domain;

internal sealed class WealthDataEntity
{
    public static Exception FutureDateError(string propertyName) =>
        new ArgumentOutOfRangeException(propertyName, "Date value for updates cannot be in the future.");

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string[] Categories { get; private set; }

    public SortedDictionary<DateTimeOffset, decimal> DailyValues { get; }

    public WealthDataEntity(
        int id,
        string name,
        string[]? categories = null,
        SortedDictionary<DateTimeOffset, decimal>? dailyValues = null)
    {
        Id = id;
        Name = name;
        Categories = categories ?? [];
        DailyValues = dailyValues ?? [];
    }

    public void SetId(int id) => Id = id;

    internal WealthDataEntity UpdateEntry(string name, string[] categories)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Categories = categories;

        return this;
    }

    internal WealthDataEntity AddDailyValue(DateTimeOffset date, decimal value)
    {
        OnValidDate(date, d => DailyValues[d.Date] = value);
        return this;
    }

    internal void RemoveDailyValue(DateTimeOffset date) =>
        OnValidDate(date, d => DailyValues.Remove(d.Date));

    internal WealthDataEntity RemoveDailyValues(IEnumerable<DateTimeOffset> dates)
    {
        dates.ForEach(date => RemoveDailyValue(date));
        return this;
    }

    internal decimal GetLatestValue() => DailyValues.LastOrDefault().Value;

    internal decimal GetLatestValueFor(DateTimeOffset date) =>
        date.IsFutureDate()
            ? throw FutureDateError(nameof(date))
            : DailyValues.Where(x => x.Key <= date.Date).LastOrDefault().Value;

    private static void OnValidDate(DateTimeOffset date, Action<DateTimeOffset> action) =>
        date.IsFutureDate().IfTrueOrElse(
            () => throw FutureDateError(nameof(date)),
            () => action(date));
}
