using Apps.Repositories;
using D20Tek.Functional;

namespace WealthTracker.Domain;

internal sealed class WealthDataEntity : IEntity
{
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Id = id;
        Name = name;
        Categories = categories ?? [];
        DailyValues = dailyValues ?? [];
    }

    public void SetId(int id) => Id = id;

    internal void UpdateEntry(string name, string[] categories)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Categories = categories;
    }

    internal void AddDailyValue(DateTimeOffset date, decimal value) =>
        OnValidDate(date, d => DailyValues[d.Date] = value);

    internal void RemoveDailyValue(DateTimeOffset date) =>
        OnValidDate(date, d => DailyValues.Remove(d.Date));

    internal decimal GetLatestValue() => DailyValues.LastOrDefault().Value;

    internal decimal GetLatestValueFor(DateTimeOffset date) =>
        date.IsFutureDate()
            ? throw Constants.FutureDateError(nameof(date))
            : DailyValues.Where(x => x.Key <= date.Date).LastOrDefault().Value;

    private static void OnValidDate(DateTimeOffset date, Action<DateTimeOffset> action) =>
        date.IsFutureDate().IfTrueOrElse(
            () => throw Constants.FutureDateError(nameof(date)),
            () => action(date));
}
