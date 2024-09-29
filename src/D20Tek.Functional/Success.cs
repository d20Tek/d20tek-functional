namespace D20Tek.Functional;

public sealed class Success<T> : Result<T>
    where T : notnull
{
    private readonly T _value;

    internal Success(T value) => _value = value;

    protected override TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error[], TResult> onFailure) =>
            onSuccess(_value);        
}
