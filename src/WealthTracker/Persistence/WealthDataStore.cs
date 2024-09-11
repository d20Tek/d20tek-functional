namespace WealthTracker.Persistence;

internal sealed class WealthDataStore
{
    public int LastId { get; set; } = 0;

    public List<WealthDataEntry> Entities { get; init; } = [];

    internal int GetNextId() => ++LastId;
}
