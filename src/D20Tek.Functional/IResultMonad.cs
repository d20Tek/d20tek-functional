namespace D20Tek.Functional;

public interface IResultMonad
{
    public object GetValue();

    public Error[] GetErrors();

    public bool IsSuccess { get; }

    public bool IsFailure { get; }
}
