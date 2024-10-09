namespace D20Tek.Functional;

public sealed class Failure<T> : Result<T>
    where T : notnull
{
    private readonly Error[] _errors;

    internal Failure(Error[] errors) => _errors = errors;

    public override TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error[], TResult> onFailure) =>
            onFailure(_errors);

    public static implicit operator Failure<T>(Error[] errors) => new(errors);
    public static implicit operator Failure<T>(Error error) => new([error]);
    public static implicit operator Failure<T>(Exception ex) => new([Error.Exception(ex)]);
}
