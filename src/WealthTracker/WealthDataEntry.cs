namespace WealthTracker;

internal sealed class WealthDataEntry
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string[] Categories { get; private set; }

    public SortedDictionary<DateTimeOffset, decimal> DailyValues { get; }

    public WealthDataEntry(
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

    internal void SetId(int id) => Id = id;

    internal void UpdateEntry(string name, string[] categories)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Categories = categories;
    }

    internal void AddDailyValue(DateTimeOffset date, decimal value) => 
        DailyValues[date.Date] = value;

    internal void RemoveDailyValue(DateTimeOffset date) =>
        DailyValues.Remove(date.Date);

    internal decimal GetLatestValue() => DailyValues.LastOrDefault().Value;
}
