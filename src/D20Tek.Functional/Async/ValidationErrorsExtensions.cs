namespace D20Tek.Functional.Async;

public static class ValidationErrorsExtensions
{
    public static async Task<ValidationErrors> AddIfErrorAsync(
        this ValidationErrors errors,
        Func<Task<bool>> check, Error error)
    {
        if (await check()) errors.AddError(error);
        return errors;
    }

    public static async Task<ValidationErrors> AddIfErrorAsync(
        this Task<ValidationErrors> task,
        Func<Task<bool>> check, Error error) =>
        await (await task).AddIfErrorAsync(check, error);

    public static async Task<ValidationErrors> AddIfErrorAsync(
        this ValidationErrors errors,
        Func<Task<bool>> check,
        string code,
        string message) =>
        await errors.AddIfErrorAsync(check, Error.Validation(code, message));

    public static async Task<ValidationErrors> AddIfErrorAsync(
        this Task<ValidationErrors> task,
        Func<Task<bool>> check,
        string code,
        string message) =>
        await (await task).AddIfErrorAsync(check, code, message);

    public static async Task<Result<T>> MapAsync<T>(this ValidationErrors errors, Func<Task<T>> onSuccess)
        where T : notnull =>
        errors.HasErrors ? errors.ToFailure<T>() : await onSuccess();

    public static async Task<Result<T>> MapAsync<T>(this Task<ValidationErrors> task, Func<Task<T>> onSuccess)
        where T : notnull =>
        await (await task).MapAsync(onSuccess);
}
