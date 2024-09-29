namespace D20Tek.Functional;

public abstract class Result<T>
    where T : notnull
{
    public static Result<T> Success(T value) => new Success<T>(value);

    public static Result<T> Failure(Error[] errors) => new Failure<T>(errors);

    public static Result<T> Failure(Error error) => new Failure<T>([error]);

    public bool IsSuccess => this is Success<T>;

    public bool IsFailure => this is Failure<T>;

    protected abstract TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error[], TResult> onFailure);
}
