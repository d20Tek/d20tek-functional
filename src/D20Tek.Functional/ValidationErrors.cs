namespace D20Tek.Functional;

/// <summary>
/// A fluent builder for accumulating validation errors before producing a <see cref="Result{T}"/>.
/// Chain multiple <see cref="AddIfError(Func{bool}, Error)"/> calls to collect all validation failures,
/// then call <see cref="Map{T}"/> or <see cref="Bind{T}"/> to either produce a success value or a failure containing all errors.
/// </summary>
public sealed class ValidationErrors
{
    private readonly IList<Error> _errors = [];

    /// <summary>Gets whether any validation errors have been accumulated.</summary>
    public bool HasErrors => _errors.Count > 0;

    private ValidationErrors() { }

    /// <summary>
    /// Creates a new empty <see cref="ValidationErrors"/> instance to begin accumulating checks.
    /// </summary>
    public static ValidationErrors Create() => new();

    /// <summary>
    /// Evaluates <paramref name="check"/>; if it returns <c>true</c>, adds <paramref name="error"/> to the error list.
    /// Returns this instance for fluent chaining.
    /// </summary>
    /// <param name="check">A function that returns <c>true</c> when the validation fails.</param>
    /// <param name="error">The error to add when validation fails.</param>
    public ValidationErrors AddIfError(Func<bool> check, Error error)
    {
        if (check()) _errors.Add(error);
        return this;
    }

    /// <summary>
    /// Evaluates <paramref name="check"/>; if it returns <c>true</c>, adds a validation error with the given code and message.
    /// </summary>
    /// <param name="check">A function that returns <c>true</c> when the validation fails.</param>
    /// <param name="code">The error code to use.</param>
    /// <param name="message">The error message to use.</param>
    public ValidationErrors AddIfError(Func<bool> check, string code, string message) =>
        AddIfError(check, Error.Validation(code, message));

    internal void AddError(Error error) => _errors.Add(error);

    /// <summary>
    /// If no errors have been accumulated, invokes <paramref name="onSuccess"/> and wraps the result in a success Result.
    /// Otherwise returns a failure Result containing all accumulated errors.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="onSuccess">A function producing the success value.</param>
    public Result<T> Map<T>(Func<T> onSuccess) where T : notnull => HasErrors ? ToFailure<T>() : onSuccess();

    /// <summary>
    /// If no errors have been accumulated, invokes <paramref name="onSuccess"/> which itself returns a Result.
    /// Otherwise returns a failure Result containing all accumulated errors.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="onSuccess">A function producing a Result.</param>
    public Result<T> Bind<T>(Func<Result<T>> onSuccess) where T : notnull => HasErrors ? ToFailure<T>() : onSuccess();

    /// <summary>
    /// Returns the accumulated errors as an array.
    /// </summary>
    public Error[] ToArray() => [.. _errors];

    /// <summary>
    /// Converts the accumulated errors into a failure <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The result value type.</typeparam>
    public Result<T> ToFailure<T>() where T : notnull => Result<T>.Failure([.. _errors]);
}
