namespace D20Tek.Functional;

public sealed class Choice<T1, T2, T3, T4, T5>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
{
    private readonly object _value;

    public Choice(T1 value) => _value = value;

    public Choice(T2 value) => _value = value;

    public Choice(T3 value) => _value = value;

    public Choice(T4 value) => _value = value;

    public Choice(T5 value) => _value = value;

    public bool IsChoice1 => _value is T1;

    public bool IsChoice2 => _value is T2;

    public bool IsChoice3 => _value is T3;

    public bool IsChoice4 => _value is T4;

    public bool IsChoice5 => _value is T5;

    public TResult Match<TResult>(
        Func<T1, TResult> func1,
        Func<T2, TResult> func2,
        Func<T3, TResult> func3,
        Func<T4, TResult> func4, 
        Func<T5, TResult> func5) =>
        _value switch
        {
            T1 => func1((T1)_value),
            T2 => func2((T2)_value),
            T3 => func3((T3)_value),
            T4 => func4((T4)_value),
            T5 => func5((T5)_value),
            _ => throw new ArgumentOutOfRangeException("value", "Invalid Choice type")
        };

    public Choice<T1, T2, T3, T4, T5> Iter(
        Action<T1> action1,
        Action<T2> action2,
        Action<T3> action3,
        Action<T4> action4,
        Action<T5> action5)
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
            case T4:
                action4((T4)_value);
                break;
            case T5:
                action5((T5)_value);
                break;
            default:
                throw new ArgumentOutOfRangeException("value", "Invalid Choice type");
        }

        return this;
    }

    public Choice<TResult, T2, T3, T4, T5> Bind<TResult>(Func<T1, Choice<TResult, T2, T3, T4, T5>> bindFunc)
        where TResult : notnull =>
        Match(
            t1 => bindFunc(t1),
            t2 => new Choice<TResult, T2, T3, T4, T5>(t2),
            t3 => new Choice<TResult, T2, T3, T4, T5>(t3),
            t4 => new Choice<TResult, T2, T3, T4, T5>(t4),
            t5 => new Choice<TResult, T2, T3, T4, T5>(t5));

    public T1 GetChoice1() => (T1)_value;

    public T2 GetChoice2() => (T2)_value;

    public T3 GetChoice3() => (T3)_value;

    public T4 GetChoice4() => (T4)_value;

    public T5 GetChoice5() => (T5)_value;

    public Choice<TResult, T2, T3, T4, T5> Map<TResult>(Func<T1, TResult> mapFunc) where TResult : notnull => 
        Match(
            t1 => new Choice<TResult, T2, T3, T4, T5>(mapFunc(t1)),
            t2 => new Choice<TResult, T2, T3, T4, T5>(t2),
            t3 => new Choice<TResult, T2, T3, T4, T5>(t3),
            t4 => new Choice<TResult, T2, T3, T4, T5>(t4),
            t5 => new Choice<TResult, T2, T3, T4, T5>(t5));

    public override string ToString() => $"Choice<{_value.GetType().Name}>(value = {_value})";
}
