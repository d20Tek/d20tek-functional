namespace D20Tek.Minimal.Functional;

public abstract class Maybe<T>
{
}

public class Something<T> : Maybe<T>
{
    public T Value { get; init; }

    public Something(T value)
    {
        Value = value;
    }
}

public class Nothing<T> : Maybe<T>
{
}
