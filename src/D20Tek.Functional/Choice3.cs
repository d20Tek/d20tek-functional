namespace D20Tek.Functional;

public sealed class Choice<T1, T2, T3>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
{
    private readonly object _value;

    public Choice(T1 value) => _value = value;

    public Choice(T2 value) => _value = value;

    public Choice(T3 value) => _value = value;

    public bool IsChoice1 => _value is T1;

    public bool IsChoice2 => _value is T2;

    public bool IsChoice3 => _value is T3;

    public TResult Match<TResult>(Func<T1, TResult> func1, Func<T2, TResult> func2, Func<T3, TResult> func3) =>
        _value switch
        {
            T1 => func1((T1)_value),
            T2 => func2((T2)_value),
            T3 => func3((T3)_value),
            _ => throw Constants.ChoiceValueException
        };

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

    public Choice<TResult, T2, T3> Bind<TResult>(Func<T1, Choice<TResult, T2, T3>> bindFunc)
        where TResult : notnull =>
        Match(t1 => bindFunc(t1), t2 => new Choice<TResult, T2, T3>(t2), t3 => new Choice<TResult, T2, T3>(t3));

    public T1 GetChoice1() => (T1)_value;

    public T2 GetChoice2() => (T2)_value;

    public T3 GetChoice3() => (T3)_value;

    public Choice<TResult, T2, T3> Map<TResult>(Func<T1, TResult> mapFunc) where TResult : notnull => 
        Match(
            t1 => new Choice<TResult, T2, T3>(mapFunc(t1)),
            t2 => new Choice<TResult, T2, T3>(t2),
            t3 => new Choice<TResult, T2, T3>(t3));

    public override string ToString() => Constants.ChoiceFormatString(_value.GetType(), _value);
}
