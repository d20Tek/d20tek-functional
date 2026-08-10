namespace D20Tek.Functional.Async;

/// <summary>
/// An async-aware discriminated union of two types. Provides <see cref="MatchAsync{TResult}"/>,
/// <see cref="BindAsync{TResult}"/>, <see cref="MapAsync{TResult}"/>, and <see cref="IterAsync"/>
/// for asynchronous pattern matching and transformation.
/// </summary>
/// <typeparam name="T1">The first possible type (primary path for Bind/Map).</typeparam>
/// <typeparam name="T2">The second possible type.</typeparam>
public sealed class ChoiceAsync<T1, T2>
    where T1 : notnull
    where T2 : notnull
{
    private readonly object _value;

    /// <summary>Creates a ChoiceAsync holding a value of the first type.</summary>
    public ChoiceAsync(T1 value) => _value = value;

    /// <summary>Creates a ChoiceAsync holding a value of the second type.</summary>
    public ChoiceAsync(T2 value) => _value = value;

    /// <summary>Gets whether the stored value is of the first type.</summary>
    public bool IsChoice1 => _value is T1;

    /// <summary>Gets whether the stored value is of the second type.</summary>
    public bool IsChoice2 => _value is T2;

    /// <summary>
    /// Asynchronously pattern-matches on the choice, invoking the appropriate async handler.
    /// </summary>
    public async Task<TResult> MatchAsync<TResult>(Func<T1, Task<TResult>> func1, Func<T2, Task<TResult>> func2) =>
        _value switch
        {
            T1 => await func1((T1)_value),
            T2 => await func2((T2)_value),
            _ => throw Constants.ChoiceValueException
        };

    /// <summary>
    /// Executes an async side-effect action based on which type is stored, then returns this choice.
    /// </summary>
    public async Task<ChoiceAsync<T1, T2>> IterAsync(Func<T1, Task> action1, Func<T2, Task> action2)
    {
        switch (_value)
        {
            case T1:
                await action1((T1)_value);
                break;
            case T2:
                await action2((T2)_value);
                break;
            default:
                throw Constants.ChoiceValueException;
        }

        return this;
    }

    /// <summary>
    /// Async monadic bind on the first type. The second type passes through unchanged.
    /// </summary>
    public async Task<Choice<TResult, T2>> BindAsync<TResult>(Func<T1, Task<Choice<TResult, T2>>> bindFunc)
        where TResult : notnull =>
        await MatchAsync(async (t1) => await bindFunc(t1), t2 => Task.FromResult(new Choice<TResult, T2>(t2)));

    /// <summary>Extracts the value as the first type.</summary>
    public T1 GetChoice1() => (T1)_value;

    /// <summary>Extracts the value as the second type.</summary>
    public T2 GetChoice2() => (T2)_value;

    /// <summary>
    /// Async map on the first type's value. The second type passes through unchanged.
    /// </summary>
    public async Task<Choice<TResult, T2>> MapAsync<TResult>(Func<T1, Task<TResult>> mapFunc)
        where TResult : notnull =>
        await MatchAsync(
            async (t1) => new Choice<TResult, T2>(await mapFunc(t1)),
            t2 => Task.FromResult(new Choice<TResult, T2>(t2)));

    /// <inheritdoc/>
    public override string ToString() => Constants.ChoiceAsyncFormatString(_value.GetType(), _value);
}
