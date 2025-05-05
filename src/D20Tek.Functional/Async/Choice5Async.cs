namespace D20Tek.Functional.Async;

public sealed class ChoiceAsync<T1, T2, T3, T4, T5>
    where T1 : notnull
    where T2 : notnull
    where T3 : notnull
    where T4 : notnull
    where T5 : notnull
{
    private readonly object _value;

    public ChoiceAsync(T1 value) => _value = value;

    public ChoiceAsync(T2 value) => _value = value;

    public ChoiceAsync(T3 value) => _value = value;

    public ChoiceAsync(T4 value) => _value = value;

    public ChoiceAsync(T5 value) => _value = value;

    public bool IsChoice1 => _value is T1;

    public bool IsChoice2 => _value is T2;

    public bool IsChoice3 => _value is T3;

    public bool IsChoice4 => _value is T4;

    public bool IsChoice5 => _value is T5;

    public async Task<TResult> MatchAsync<TResult>(
        Func<T1, Task<TResult>> func1,
        Func<T2, Task<TResult>> func2,
        Func<T3, Task<TResult>> func3,
        Func<T4, Task<TResult>> func4, 
        Func<T5, Task<TResult>> func5) =>
        _value switch
        {
            T1 => await func1((T1)_value),
            T2 => await func2((T2)_value),
            T3 => await func3((T3)_value),
            T4 => await func4((T4)_value),
            T5 => await func5((T5)_value),
            _ => throw new ArgumentOutOfRangeException(nameof(_value), "Invalid Choice type")
        };

    public async Task<ChoiceAsync<T1, T2, T3, T4, T5>> IterAsync(
        Func<T1, Task> action1,
        Func<T2, Task> action2,
        Func<T3, Task> action3,
        Func<T4, Task> action4,
        Func<T5, Task> action5)
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
            case T5:
                await action5((T5)_value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_value), "Invalid Choice type");
        }

        return this;
    }

    public async Task<ChoiceAsync<TResult, T2, T3, T4, T5>> BindAsync<TResult>(
        Func<T1, Task<ChoiceAsync<TResult, T2, T3, T4, T5>>> bindFunc)
        where TResult : notnull =>
        await MatchAsync(
            async (t1) => await bindFunc(t1),
            t2 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t2)),
            t3 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t3)),
            t4 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t4)),
            t5 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t5)));

    public T1 GetChoice1() => (T1)_value;

    public T2 GetChoice2() => (T2)_value;

    public T3 GetChoice3() => (T3)_value;

    public T4 GetChoice4() => (T4)_value;

    public T5 GetChoice5() => (T5)_value;

    public async Task<ChoiceAsync<TResult, T2, T3, T4, T5>> MapAsync<TResult>(Func<T1, Task<TResult>> mapFunc)
        where TResult : notnull => 
        await MatchAsync(
            async (t1) => new ChoiceAsync<TResult, T2, T3, T4, T5>(await mapFunc(t1)),
            t2 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t2)),
            t3 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t3)),
            t4 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t4)),
            t5 => Task.FromResult(new ChoiceAsync<TResult, T2, T3, T4, T5>(t5)));

    public override string ToString() => $"ChoiceAsync<{_value.GetType().Name}>(value = {_value})";
}
