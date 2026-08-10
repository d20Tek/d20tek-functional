namespace D20Tek.Functional;

/// <summary>
/// Extension methods for <see cref="bool"/> that enable functional branching without if/else statements.
/// </summary>
public static class BooleanExtensions
{
    /// <summary>
    /// Evaluates one of two functions based on the boolean value and returns the result.
    /// Replaces ternary expressions with a fluent, pipeline-friendly call.
    /// </summary>
    /// <typeparam name="TOut">The return type of both branches.</typeparam>
    /// <param name="condition">The boolean condition to evaluate.</param>
    /// <param name="thenFunc">Function to invoke when <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="elseFunc">Function to invoke when <paramref name="condition"/> is <c>false</c>.</param>
    public static TOut IfTrueOrElse<TOut>(this bool condition, Func<TOut> thenFunc, Func<TOut> elseFunc) =>
        condition ? thenFunc() : elseFunc();

    /// <summary>
    /// Executes one of two actions based on the boolean value.
    /// The <paramref name="elseAction"/> is optional and defaults to no-op.
    /// </summary>
    /// <param name="condition">The boolean condition to evaluate.</param>
    /// <param name="thenAction">Action to execute when <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="elseAction">Optional action to execute when <paramref name="condition"/> is <c>false</c>.</param>
    public static void IfTrueOrElse(this bool condition, Action thenAction, Action? elseAction = null)
    {
        if (condition)
            thenAction();
        else
            elseAction?.Invoke();
    }
}
