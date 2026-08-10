namespace D20Tek.Functional;

/// <summary>
/// General-purpose functional programming extension methods that work on any type.
/// Includes pipeline operators (Pipe), parallel composition (Fork), alternative selection (Alt),
/// and iterative looping (IterateUntil).
/// </summary>
public static class FunctionalExtensions
{
    /// <summary>
    /// Tries each function in <paramref name="args"/> in order, returning the first non-null result.
    /// Returns <c>null</c> if all functions return null. Useful for fallback/chain-of-responsibility patterns.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <param name="instance">The input value passed to each function.</param>
    /// <param name="args">An array of functions to try in sequence.</param>
    public static TOut? Alt<TIn, TOut>(this TIn instance, params Func<TIn, TOut>[] args) =>
        args.Select(x => x(instance)).FirstOrDefault(x => x != null);

    /// <summary>
    /// Applies two independent functions to the same input, then combines their results.
    /// Inspired by the "fork" combinator for parallel decomposition of a value.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="T1">The result type of the first function.</typeparam>
    /// <typeparam name="T2">The result type of the second function.</typeparam>
    /// <typeparam name="TOut">The combined output type.</typeparam>
    /// <param name="instance">The input value.</param>
    /// <param name="f1">First projection function.</param>
    /// <param name="f2">Second projection function.</param>
    /// <param name="fOut">Combining function that merges both projections.</param>
    public static TOut Fork<TIn, T1, T2, TOut>(
        this TIn instance,
        Func<TIn, T1> f1,
        Func<TIn, T2> f2,
        Func<T1, T2, TOut> fOut) =>
        fOut(f1(instance), f2(instance));

    /// <summary>
    /// Applies two independent functions to the same input, then combines their results via an action.
    /// </summary>
    /// <typeparam name="TIn">The input type.</typeparam>
    /// <typeparam name="T1">The result type of the first function.</typeparam>
    /// <typeparam name="T2">The result type of the second function.</typeparam>
    /// <param name="instance">The input value.</param>
    /// <param name="f1">First projection function.</param>
    /// <param name="f2">Second projection function.</param>
    /// <param name="fOut">Action that consumes both projections.</param>
    public static void Fork<TIn, T1, T2>(
        this TIn instance,
        Func<TIn, T1> f1,
        Func<TIn, T2> f2,
        Action<T1, T2> fOut) =>
        fOut(f1(instance), f2(instance));

    /// <summary>
    /// Passes the instance through a transformation function, enabling fluent pipelines.
    /// Equivalent to F#'s pipe-forward operator (<c>|&gt;</c>).
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="instance">The value to pipe.</param>
    /// <param name="func">The transformation function.</param>
    public static TResult Pipe<T, TResult>(this T instance, Func<T, TResult> func) => func(instance);

    /// <summary>
    /// Executes a side-effect action on the instance and returns it unchanged.
    /// Useful for logging, debugging, or triggering effects within a pipeline.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="instance">The value to pass through.</param>
    /// <param name="action">The side-effect action to execute.</param>
    public static T Pipe<T>(this T instance, Action<T> action)
    {
        action(instance);
        return instance;
    }

    /// <summary>
    /// Repeatedly applies <paramref name="updateFunction"/> to the value until <paramref name="endCondition"/> is met.
    /// Use for iterative computations expressed functionally without mutable loops.
    /// </summary>
    /// <typeparam name="T">The type being iterated.</typeparam>
    /// <param name="instance">The initial value.</param>
    /// <param name="updateFunction">A function producing the next iteration value.</param>
    /// <param name="endCondition">A predicate that returns <c>true</c> when iteration should stop.</param>
    public static T IterateUntil<T>(this T instance, Func<T, T> updateFunction, Func<T, bool> endCondition)
    {
        var currentThis = instance;

        try
        {
            while (!endCondition(currentThis))
            {
                currentThis = updateFunction(currentThis);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            throw;
        }

        return currentThis;
    }

    /// <summary>
    /// Repeatedly applies <paramref name="updateFunction"/> (which may fail) until <paramref name="endCondition"/> is met
    /// or a failure occurs. Returns the final <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type being iterated.</typeparam>
    /// <param name="instance">The initial value.</param>
    /// <param name="updateFunction">A function producing the next Result on each iteration.</param>
    /// <param name="endCondition">A predicate that returns <c>true</c> when iteration should stop.</param>
    public static Result<T> IterateUntil<T>(
        this T instance,
        Func<T, Result<T>> updateFunction,
        Func<T, bool> endCondition)
        where T : notnull
    {
        var currentThis = Result<T>.Success(instance);

        try
        {
            while (currentThis is Success<T> s && !endCondition(s))
            {
                currentThis = updateFunction(s);
            }
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex);
        }

        return currentThis;
    }
}
