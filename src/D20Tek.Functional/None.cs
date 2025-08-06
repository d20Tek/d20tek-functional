namespace D20Tek.Functional;

internal sealed class NoneOptional<T> : Optional<T>
    where T : notnull
{
    internal NoneOptional()
    {
    }

    public override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onNone();
}
