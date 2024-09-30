namespace D20Tek.Functional;

public static class ResultExtensions
{
    public static Result<T> Flatten<T>(this Result<Result<T>> result) where T : notnull =>
        result.Match(someResult => someResult, e => Result<T>.Failure(e));
}
