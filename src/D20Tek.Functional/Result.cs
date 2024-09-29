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

    public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> bind) where TResult : notnull =>
        Match(v => bind(v), e => Result<TResult>.Failure(e));

    public int Count() => Match(_ => 1, _ => 0);

    public T DefaultValue(T defaultArg) => Match(v => v, _ => defaultArg);

    public T DefaultWith(Func<T> func) => Match(v => v, _ => func());

    public bool Exists(Func<T, bool> predicate) => Match(v => predicate(v), _ => false);

    public Result<T> Filter(Func<T, bool> predicate) =>
        Match(
            v => predicate(v) ? Result<T>.Success(v)
                              : Result<T>.Failure(Error.NotFound("Filter.Error", "No filtered items found.")),
            e => Result<T>.Failure(e));

    public TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func) where TResult : notnull =>
        Match(v => func(initial, v), _ => initial);

    public TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func) where TResult : notnull =>
        Match(v => func(v, initial), _ => initial);

    public bool ForAll(Func<T, bool> predicate) => Match(v => predicate(v), _ => true);

    public T GetValue() => Match(v => v, _ => throw new ArgumentNullException("Value"));

    public Error[] GetErrors() => Match(_ => [], e => e);
}
