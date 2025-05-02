namespace D20Tek.Functional.Async;

public sealed class ChoiceAsync<T1, T2>
    where T1 : notnull
    where T2 : notnull
{
    private readonly object _value;

    public ChoiceAsync(T1 value) => _value = value;

    public ChoiceAsync(T2 value) => _value = value;

    public bool IsChoice1 => _value is T1;

    public bool IsChoice2 => _value is T2;

    public async Task<TResult> MatchAsync<TResult>(Func<T1, Task<TResult>> func1, Func<T2, Task<TResult>> func2) =>
        _value switch
        {
            T1 => await func1((T1)_value),
            T2 => await func2((T2)_value),
            _ => throw new ArgumentOutOfRangeException(nameof(_value), "Invalid Choice type")
        };

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
                throw new ArgumentOutOfRangeException(nameof(_value), "Invalid Choice type");
        }

        return this;
    }

    public async Task<Choice<TResult, T2>> BindAsync<TResult>(
        Func<T1, Task<Choice<TResult, T2>>> bindFunc)
        where TResult : notnull =>
        await MatchAsync(async (t1) => await bindFunc(t1), t2 => Task.FromResult(new Choice<TResult, T2>(t2)));

    public T1 GetChoice1() => (T1)_value;

    public T2 GetChoice2() => (T2)_value;

    public async Task<Choice<TResult, T2>> MapAsync<TResult>(
        Func<T1, Task<TResult>> mapFunc)
        where TResult : notnull =>
        await MatchAsync(
            async (t1) => new Choice<TResult, T2>(await mapFunc(t1)),
            t2 => Task.FromResult(new Choice<TResult, T2>(t2)));

    public override string ToString() => $"ChoiceAsync<{_value.GetType().Name}>(value = {_value})";
}
