namespace D20Tek.Minimal.Functional;

public abstract class Maybe<T>
{
    public static implicit operator Maybe<T>(T @this) => @this.ToMaybe();
}

public class Something<T> : Maybe<T>
{
    public T Value { get; init; }

    public Something(T value)
    {
        Value = value;
    }

    public static implicit operator Something<T>(T @this) => new(@this);
    public static implicit operator T(Something<T> @this) => @this.Value;
}

public class Nothing<T> : Maybe<T>
{
}

public class Exceptional<T> : Maybe<T>
{
    public Exceptional(Exception e)
    {
        ErrorMessage = e;
    }

    public Exception ErrorMessage { get; set; }
}
