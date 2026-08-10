namespace D20Tek.Functional.Async;

/// <summary>
/// Async extension methods for <see cref="ValidationErrors"/> enabling asynchronous validation checks
/// and result conversions.
/// </summary>
public static class ValidationErrorsExtensions
{
    /// <summary>
    /// Asynchronously checks a condition and adds an error if the check returns <c>true</c>.
    /// </summary>
    public static async Task<ValidationErrors> AddIfErrorAsync(
        this ValidationErrors errors, Func<Task<bool>> check, Error error)
    {
        if (await check()) errors.AddError(error);
        return errors;
    }

    /// <summary>
    /// Awaits a ValidationErrors task, then asynchronously checks a condition and adds an error.
    /// </summary>
    public static async Task<ValidationErrors> AddIfErrorAsync(
        this Task<ValidationErrors> task, Func<Task<bool>> check, Error error) =>
        await (await task).AddIfErrorAsync(check, error);

    /// <summary>
    /// Asynchronously checks a condition and adds a validation error with the given code and message.
    /// </summary>
    public static async Task<ValidationErrors> AddIfErrorAsync(
        this ValidationErrors errors, Func<Task<bool>> check, string code, string message) =>
        await errors.AddIfErrorAsync(check, Error.Validation(code, message));

    /// <summary>
    /// Awaits a ValidationErrors task, then asynchronously checks a condition and adds a validation error.
    /// </summary>
    public static async Task<ValidationErrors> AddIfErrorAsync(
        this Task<ValidationErrors> task, Func<Task<bool>> check, string code, string message) =>
        await (await task).AddIfErrorAsync(check, code, message);

    /// <summary>
    /// If no errors are present, asynchronously invokes <paramref name="onSuccess"/> to produce a success result;
    /// otherwise returns a failure result containing the accumulated errors.
    /// </summary>
    public static async Task<Result<T>> MapAsync<T>(this ValidationErrors errors, Func<Task<T>> onSuccess)
        where T : notnull =>
        errors.HasErrors ? errors.ToFailure<T>() : await onSuccess();

    /// <summary>
    /// Awaits a ValidationErrors task, then asynchronously maps to a result.
    /// </summary>
    public static async Task<Result<T>> MapAsync<T>(this Task<ValidationErrors> task, Func<Task<T>> onSuccess)
        where T : notnull =>
        await (await task).MapAsync(onSuccess);

    /// <summary>
    /// If no errors are present, asynchronously invokes <paramref name="bind"/> which itself returns a Result;
    /// otherwise returns a failure result containing the accumulated errors.
    /// </summary>
    public static async Task<Result<T>> BindAsync<T>(this ValidationErrors errors, Func<Task<Result<T>>> bind) 
        where T : notnull =>
        errors.HasErrors ? errors.ToFailure<T>() : await bind();

    /// <summary>
    /// Awaits a ValidationErrors task, then asynchronously binds to a result.
    /// </summary>
    public static async Task<Result<T>> BindAsync<T>(this Task<ValidationErrors> task, Func<Task<Result<T>>> bind)
        where T : notnull =>
        await (await task).BindAsync(bind);
}
