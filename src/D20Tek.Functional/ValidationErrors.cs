namespace D20Tek.Functional;

public sealed class ValidationErrors
{
    private readonly IList<Error> _errors = [];

    public bool HasErrors => _errors.Count > 0;

    private ValidationErrors() { }

    public static ValidationErrors Create() => new();

    public ValidationErrors AddIfError(Func<bool> check, Error error)
    {
        if (check()) _errors.Add(error);
        return this;
    }

    public ValidationErrors AddIfError(Func<bool> check, string code, string message) =>
        AddIfError(check, Error.Validation(code, message));

    internal void AddError(Error error) => _errors.Add(error);

    public Result<T> Map<T>(Func<T> onSuccess) where T : notnull => HasErrors ? ToFailure<T>() : onSuccess();

    public Error[] ToArray() => [.. _errors];

    public Result<T> ToFailure<T>() where T : notnull => Result<T>.Failure([.. _errors]);
}
