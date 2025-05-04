namespace D20Tek.Functional.Async;

public static class FunctionalAsyncExtensions
{
    public static async Task<TOut?> AltAsync<TIn, TOut>(this Task<TIn> instance, params Func<TIn, Task<TOut>>[] args)
    {
        var value = await instance;
        foreach (var func in args)
        {
            var result = await func(value);
            if (result != null)
                return result;
        }

        return default;
    }

    public static async Task<TOut> ForkAsync<TIn, T1, T2, TOut>(
        this Task<TIn> instance,
        Func<TIn, Task<T1>> f1,
        Func<TIn, Task<T2>> f2,
        Func<T1, T2, Task<TOut>> fOut)
    {
        var value = await instance;
        return await fOut(await f1(value), await f2(value));
    }

    public static async Task ForkAsync<TIn, T1, T2>(
        this Task<TIn> instance,
        Func<TIn, Task<T1>> f1,
        Func<TIn, Task<T2>> f2,
        Func<T1, T2, Task> fOut)
    {
        var value = await instance;
        await fOut(await f1(value), await f2(value));
    }

    public static async Task<TResult> PipeAsync<T, TResult>(this Task<T> instance, Func<T, Task<TResult>> func) => 
        await func(await instance);

    public static async Task<T> PipeAsync<T>(this Task<T> instance, Func<T, Task> action)
    {
        var value = await instance;
        await action(value);
        return value;
    }

    public static async Task<T> IterateUntilAsync<T>(
        this Task<T> instance, Func<T, Task<T>> updateFunction, Func<T, Task<bool>> endCondition)
    {
        var currentThis = await instance;

        try
        {
            while (!await endCondition(currentThis))
            {
                currentThis = await updateFunction(currentThis);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            throw;
        }

        return currentThis;
    }

    public static async Task<Result<T>> IterateUntilAsync<T>(
        this Task<T> instance, Func<T, Task<Result<T>>> updateFunction, Func<T, Task<bool>> endCondition)
        where T : notnull
    {
        var currentThis = Result<T>.Success(await instance);

        try
        {
            while (currentThis is Success<T> s && !await endCondition(s))
            {
                currentThis = await updateFunction(s);
            }
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex);
        }

        return currentThis;
    }
}
