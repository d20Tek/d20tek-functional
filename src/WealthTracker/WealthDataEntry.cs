namespace WealthTracker;

internal sealed class WealthDataEntry
{
    public int Id { get; private set; }

    public string Name { get; }

    public string[] Categories { get; }

    public Dictionary<DateTimeOffset, int> DailyValues { get; }

    public WealthDataEntry(
        int id,
        string name,
        string[]? categories = null,
        Dictionary<DateTimeOffset, int>? dailyValues = null)
    {
        Id = id;
        Name = name;
        Categories = categories ?? [];
        DailyValues = dailyValues ?? [];
    }

    internal void SetId(int id) => Id = id;
}
