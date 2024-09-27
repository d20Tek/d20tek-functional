namespace D20Tek.Functional;

public sealed class None<T> : Option<T>
    where T : notnull
{
    protected override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onNone();
}
