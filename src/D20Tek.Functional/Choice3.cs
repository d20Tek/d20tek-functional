namespace D20Tek.Functional;

/// <summary>
/// A discriminated union holding exactly one value of <typeparamref name="T1"/>, <typeparamref name="T2"/>,
/// or <typeparamref name="T3"/>. Provides exhaustive pattern matching via <see cref="Match{TResult}"/>.
/// </summary>
/// <typeparam name="T1">The first possible type (primary path for Bind/Map).</typeparam>
/// <typeparam name="T2">The second possible type.</typeparam>
/// <typeparam name="T3">The third possible type.</typeparam>
public sealed class Choice<T1, T2, T3>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
{
    private readonly object _value;

    /// <summary>Creates a Choice holding a value of the first type.</summary>
    public Choice(T1 value) => _value = value;

    /// <summary>Creates a Choice holding a value of the second type.</summary>
    public Choice(T2 value) => _value = value;

    /// <summary>Creates a Choice holding a value of the third type.</summary>
    public Choice(T3 value) => _value = value;

    /// <summary>Gets whether the stored value is of the first type.</summary>
    public bool IsChoice1 => _value is T1;

    /// <summary>Gets whether the stored value is of the second type.</summary>
    public bool IsChoice2 => _value is T2;

    /// <summary>Gets whether the stored value is of the third type.</summary>
    public bool IsChoice3 => _value is T3;

    /// <summary>
    /// Exhaustively pattern-matches on the Choice, invoking the appropriate handler.
    /// </summary>
    public TResult Match<TResult>(Func<T1, TResult> func1, Func<T2, TResult> func2, Func<T3, TResult> func3) =>
        _value switch
        {
            T1 => func1((T1)_value),
            T2 => func2((T2)_value),
            T3 => func3((T3)_value),
            _ => throw Constants.ChoiceValueException
        };

    /// <summary>
    /// Executes a side-effect action based on which type is stored, then returns this Choice.
    /// </summary>
    public Choice<T1, T2, T3> Iter(Action<T1> action1, Action<T2> action2, Action<T3> action3)
    {
        switch (_value)
        {
            case T1:
                action1((T1)_value);
                break;
            case T2:
                action2((T2)_value);
                break;
            case T3:
                action3((T3)_value);
                break;
            default:
                throw Constants.ChoiceValueException;
        }

        return this;
    }

    /// <summary>
    /// Monadic bind on the first type. Non-T1 values pass through unchanged.
    /// </summary>
    public Choice<TResult, T2, T3> Bind<TResult>(Func<T1, Choice<TResult, T2, T3>> bindFunc)
        where TResult : notnull =>
        Match(t1 => bindFunc(t1), t2 => new Choice<TResult, T2, T3>(t2), t3 => new Choice<TResult, T2, T3>(t3));

    /// <summary>Extracts the value as the first type.</summary>
    public T1 GetChoice1() => (T1)_value;

    /// <summary>Extracts the value as the second type.</summary>
    public T2 GetChoice2() => (T2)_value;

    /// <summary>Extracts the value as the third type.</summary>
    public T3 GetChoice3() => (T3)_value;

    /// <summary>
    /// Maps the first type's value. Non-T1 values pass through unchanged.
    /// </summary>
    public Choice<TResult, T2, T3> Map<TResult>(Func<T1, TResult> mapFunc) where TResult : notnull => 
        Match(
            t1 => new Choice<TResult, T2, T3>(mapFunc(t1)),
            t2 => new Choice<TResult, T2, T3>(t2),
            t3 => new Choice<TResult, T2, T3>(t3));

    /// <inheritdoc/>
    public override string ToString() => Constants.ChoiceFormatString(_value.GetType(), _value);
}
