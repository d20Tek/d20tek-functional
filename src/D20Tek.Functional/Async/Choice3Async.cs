namespace D20Tek.Functional.Async;

public sealed class ChoiceAsync<T1, T2, T3>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
{
    private readonly object _value;

    public ChoiceAsync(T1 value) => _value = value;

    public ChoiceAsync(T2 value) => _value = value;

    public ChoiceAsync(T3 value) => _value = value;

    public bool IsChoice1 => _value is T1;

    public bool IsChoice2 => _value is T2;

    public bool IsChoice3 => _value is T3;

    public async Task<TResult> MatchAsync<TResult>(
        Func<T1, Task<TResult>> func1, Func<T2, Task<TResult>> func2, Func<T3, Task<TResult>> func3) =>
        _value switch
        {
            T1 => await func1((T1)_value),
            T2 => await func2((T2)_value),
            T3 => await func3((T3)_value),
            _ => throw new ArgumentOutOfRangeException(nameof(_value), "Invalid Choice type")
        };

    public async Task<ChoiceAsync<T1, T2, T3>> IterAsync(
        Func<T1, Task> action1, Func<T2, Task> action2, Func<T3, Task> action3)
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
            default:
                throw new ArgumentOutOfRangeException(nameof(_value), "Invalid Choice type");
        }

        return this;
    }

    public async Task<ChoiceAsync<TResult, T2, T3>> BindAsync<TResult>(
        Func<T1, Task<ChoiceAsync<TResult, T2, T3>>> bindFunc)
        where TResult : notnull =>
        await MatchAsync(
            async (t1) => await bindFunc(t1),
            t2 => Task.FromResult(new ChoiceAsync<TResult, T2, T3>(t2)),
            t3 => Task.FromResult(new ChoiceAsync<TResult, T2, T3>(t3)));

    public T1 GetChoice1() => (T1)_value;

    public T2 GetChoice2() => (T2)_value;

    public T3 GetChoice3() => (T3)_value;

    public async Task<ChoiceAsync<TResult, T2, T3>> MapAsync<TResult>(Func<T1, Task<TResult>> mapFunc)
        where TResult : notnull => 
        await MatchAsync(
            async (t1) => new ChoiceAsync<TResult, T2, T3>(await mapFunc(t1)),
            t2 => Task.FromResult(new ChoiceAsync<TResult, T2, T3>(t2)),
            t3 => Task.FromResult(new ChoiceAsync<TResult, T2, T3>(t3)));

    public override string ToString() => $"ChoiceAsync<{_value.GetType().Name}>(value = {_value})";
}
