namespace WealthTracker;

internal sealed class WealthDataEntry
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string[] Categories { get; private set; }

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

    internal void UpdateEntry(string name, string[] categories)
    {
        Name = name;
        Categories = categories;
    }
}
