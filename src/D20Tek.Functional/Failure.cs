namespace D20Tek.Functional;

public sealed class Failure<T> : Result<T>
    where T : notnull
{
    private readonly Error[] _errors;

    internal Failure(Error[] errors) => _errors = errors;

    protected override TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error[], TResult> onFailure) =>
            onFailure(_errors);
}
