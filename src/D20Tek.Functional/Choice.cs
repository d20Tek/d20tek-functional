namespace D20Tek.Functional;

/// <summary>
/// A discriminated union holding exactly one value of either <typeparamref name="T1"/> or <typeparamref name="T2"/>.
/// Use Choice when a value can legitimately be one of two types and you want exhaustive pattern matching
/// via <see cref="Match{TResult}"/> rather than type-checking or exceptions.
/// </summary>
/// <typeparam name="T1">The first possible type (primary/success path for Bind/Map).</typeparam>
/// <typeparam name="T2">The second possible type (alternate path).</typeparam>
public sealed class Choice<T1, T2>
    where T1 : notnull
    where T2 : notnull
{
    private readonly object _value;

    /// <summary>Creates a Choice holding a value of the first type.</summary>
    /// <param name="value">The value to store.</param>
    public Choice(T1 value) => _value = value;

    /// <summary>Creates a Choice holding a value of the second type.</summary>
    /// <param name="value">The value to store.</param>
    public Choice(T2 value) => _value = value;

    /// <summary>Gets whether the stored value is of the first type.</summary>
    public bool IsChoice1 => _value is T1;

    /// <summary>Gets whether the stored value is of the second type.</summary>
    public bool IsChoice2 => _value is T2;

    /// <summary>
    /// Exhaustively pattern-matches on the Choice, invoking the appropriate handler based on the stored type.
    /// </summary>
    /// <typeparam name="TResult">The return type of both handlers.</typeparam>
    /// <param name="func1">Handler for the first type.</param>
    /// <param name="func2">Handler for the second type.</param>
    public TResult Match<TResult>(Func<T1, TResult> func1, Func<T2, TResult> func2) =>
        _value switch
        {
            T1 => func1((T1)_value),
            T2 => func2((T2)_value),
            _ => throw Constants.ChoiceValueException
        };

    /// <summary>
    /// Executes side-effect actions based on which type is stored, then returns this Choice.
    /// </summary>
    /// <param name="action1">Action to execute if the value is of the first type.</param>
    /// <param name="action2">Action to execute if the value is of the second type.</param>
    public Choice<T1, T2> Iter(Action<T1> action1, Action<T2> action2)
    {
        switch (_value)
        {
            case T1:
                action1((T1)_value);
                break;
            case T2:
                action2((T2)_value);
                break;
            default:
                throw Constants.ChoiceValueException;
        }

        return this;
    }

    /// <summary>
    /// Monadic bind on the first type: if the stored value is <typeparamref name="T1"/>,
    /// passes it to <paramref name="bindFunc"/>. Otherwise propagates the second type unchanged.
    /// </summary>
    /// <typeparam name="TResult">The new first type after binding.</typeparam>
    /// <param name="bindFunc">A function producing a new Choice from the first type's value.</param>
    public Choice<TResult, T2> Bind<TResult>(Func<T1, Choice<TResult, T2>> bindFunc) where TResult : notnull =>
        Match(t1 => bindFunc(t1), t2 => new Choice<TResult, T2>(t2));

    /// <summary>Extracts the value as the first type. Throws <see cref="InvalidCastException"/> if it is the second type.</summary>
    public T1 GetChoice1() => (T1)_value;

    /// <summary>Extracts the value as the second type. Throws <see cref="InvalidCastException"/> if it is the first type.</summary>
    public T2 GetChoice2() => (T2)_value;

    /// <summary>
    /// Maps the first type's value using <paramref name="mapFunc"/>. The second type passes through unchanged.
    /// </summary>
    /// <typeparam name="TResult">The new first type after mapping.</typeparam>
    /// <param name="mapFunc">A function to transform the first type's value.</param>
    public Choice<TResult, T2> Map<TResult>(Func<T1, TResult> mapFunc) where TResult : notnull =>
        Match(t1 => new Choice<TResult, T2>(mapFunc(t1)), t2 => new Choice<TResult, T2>(t2));

    /// <inheritdoc/>
    public override string ToString() => Constants.ChoiceFormatString(_value.GetType(), _value);
}
