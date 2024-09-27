namespace D20Tek.Functional;

public static class OptionExtensions
{
    public static Option<T> Flatten<T>(this Option<Option<T>> option) where T : notnull =>
        option.Match(someOption => someOption, () => Option<T>.None());
}