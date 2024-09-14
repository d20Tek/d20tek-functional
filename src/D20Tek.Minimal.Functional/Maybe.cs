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

public class Failure<T> : Maybe<T>
{
    public Error Error { get; }

    public string Message => Error.ToString();

    public Failure(Error error)
    {
        Error = error;
    }

    public static implicit operator Failure<T>(Error error) => new(error);
}

public class Exceptional<T> : Maybe<T>
{
    public Exception Exception { get; init; }

    public string Message => Exception.Message.ToString();

    public Exceptional(Exception e)
    {
        Exception = e;
    }

    public static implicit operator Exceptional<T>(Exception ex) => new(ex);
}
