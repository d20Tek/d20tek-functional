namespace D20Tek.Functional.Async;

/// <summary>
/// An async-aware discriminated union of four types with async Match, Bind, Map, and Iter operations.
/// </summary>
/// <typeparam name="T1">The first possible type (primary path for Bind/Map).</typeparam>
/// <typeparam name="T2">The second possible type.</typeparam>
/// <typeparam name="T3">The third possible type.</typeparam>
/// <typeparam name="T4">The fourth possible type.</typeparam>
public sealed class ChoiceAsync<T1, T2, T3, T4>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
{
    private readonly object _value;

    /// <summary>Creates a ChoiceAsync holding a value of the first type.</summary>
    public ChoiceAsync(T1 value) => _value = value;

    /// <summary>Creates a ChoiceAsync holding a value of the second type.</summary>
    public ChoiceAsync(T2 value) => _value = value;

    /// <summary>Creates a ChoiceAsync holding a value of the third type.</summary>
    public ChoiceAsync(T3 value) => _value = value;

    /// <summary>Creates a ChoiceAsync holding a value of the fourth type.</summary>
    public ChoiceAsync(T4 value) => _value = value;

    /// <summary>Gets whether the stored value is of the first type.</summary>
    public bool IsChoice1 => _value is T1;

    /// <summary>Gets whether the stored value is of the second type.</summary>
    public bool IsChoice2 => _value is T2;

    /// <summary>Gets whether the stored value is of the third type.</summary>
    public bool IsChoice3 => _value is T3;

    /// <summary>Gets whether the stored value is of the fourth type.</summary>
    public bool IsChoice4 => _value is T4;

    /// <summary>
    /// Asynchronously pattern-matches on the choice, invoking the appropriate async handler.
    /// </summary>
    public async Task<TResult> MatchAsync<TResult>(
        Func<T1, Task<TResult>> func1,
        Func<T2, Task<TResult>> func2,
        Func<T3, Task<TResult>> func3,
        Func<T4, Task<TResult>> func4) =>
        _value switch
        {
            T1 => await func1((T1)_value),
            T2 => await func2((T2)_value),
            T3 => await func3((T3)_value),
            T4 => await func4((T4)_value),
            _ => throw Constants.ChoiceValueException
        };

    /// <summary>
    /// Executes an async side-effect action based on which type is stored, then returns this choice.
    /// </summary>
    public async Task<ChoiceAsync<T1, T2, T3, T4>> IterAsync(
        Func<T1, Task> action1,
        Func<T2, Task> action2,
        Func<T3, Task> action3,
        Func<T4, Task> action4)
    {
        switch (_value)
        {
            case T1:
                await action1((T1)_value);
                break;
            case T2:
                await action2((T2)_value);
                break;
            case T3:
                await action3((T3)_value);
                break;
            case T4:
                await action4((T4)_value);
                break;
            default:
                throw Constants.ChoiceValueException;
        }

        return this;
    }

    /// <summary>
    /// Async monadic bind on the first type. Non-T1 values pass through unchanged.
    /// </summary>
    public async Task<ChoiceAsync<TResult, T2, T3, T4>> BindAsync<TResult>(
        Func<T1, Task<ChoiceAsync<TResult, T2, T3, T4>>> bindFunc)
        where TResult : notnull =>
        await MatchAsync(
            async (t1) => await bindFunc(t1),
            t2 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4>(t2)),
            t3 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4>(t3)),
            t4 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4>(t4)));

    /// <summary>Extracts the value as the first type.</summary>
    public T1 GetChoice1() => (T1)_value;

    /// <summary>Extracts the value as the second type.</summary>
    public T2 GetChoice2() => (T2)_value;

    /// <summary>Extracts the value as the third type.</summary>
    public T3 GetChoice3() => (T3)_value;

    /// <summary>Extracts the value as the fourth type.</summary>
    public T4 GetChoice4() => (T4)_value;

    /// <summary>
    /// Async map on the first type's value. Non-T1 values pass through unchanged.
    /// </summary>
    public async Task<ChoiceAsync<TResult, T2, T3, T4>> MapAsync<TResult>(Func<T1, Task<TResult>> mapFunc)
        where TResult : notnull => 
        await MatchAsync(
            async (t1) => new ChoiceAsync<TResult, T2, T3, T4>(await mapFunc(t1)),
            t2 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4>(t2)),
            t3 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4>(t3)),
            t4 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4>(t4)));

    /// <inheritdoc/>
    public override string ToString() => Constants.ChoiceAsyncFormatString(_value.GetType(), _value);
}
