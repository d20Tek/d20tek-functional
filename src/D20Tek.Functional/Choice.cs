namespace D20Tek.Functional;

public sealed class Choice<T1, T2>
    where T1 : notnull
    where T2 : notnull
{
    private readonly object _value;

    public Choice(T1 value) => _value = value;

    public Choice(T2 value) => _value = value;

    public bool IsChoice1 => _value is T1;

    public bool IsChoice2 => _value is T2;

    public TResult Match<TResult>(Func<T1, TResult> func1, Func<T2, TResult> func2) =>
        _value switch
        {
            T1 => func1((T1)_value),
            T2 => func2((T2)_value),
            _ => throw new ArgumentOutOfRangeException("Invalid Choice type")
        };

    public void Iter(Action<T1> action1, Action<T2> action2)
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
                throw new ArgumentOutOfRangeException("Invalid Choice type");
        }
    }

    public Choice<TResult, T2> Bind<TResult>(Func<T1, Choice<TResult, T2>> bindFunc) where TResult : notnull =>
        Match(t1 => bindFunc(t1), t2 => new Choice<TResult, T2>(t2));

    public T1 GetChoice1() => (T1)_value;

    public T2 GetChoice2() => (T2)_value;

    public Choice<TResult, T2> Map<TResult>(Func<T1, TResult> mapFunc) where TResult : notnull => 
        Match(t1 => new Choice<TResult, T2>(mapFunc(t1)), t2 => new Choice<TResult, T2>(t2));

    public override string ToString() =>
        Match(
            v1 => $"Choice<{typeof(T1).Name}>(value = {v1})",
            v2 => $"Choice<{typeof(T2).Name}>(value = {v2})");
}
