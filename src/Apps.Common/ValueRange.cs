namespace Apps.Common;

internal sealed record ValueRange<T>(T Min, T Max)
    where T : struct, IComparable<T>
{
    public bool InRange(T value) => value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
}
