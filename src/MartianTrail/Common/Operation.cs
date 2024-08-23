namespace MartianTrail.Common;

internal abstract class Operation
{
}

internal class Success : Operation
{
}

internal class Failure : Operation
{
    public Failure(Exception e)
    {
        CapturedException = e;
    }

    public Exception CapturedException { get; set; }
}

internal static class SuccessOrFailureExtensions
{
    public static Operation TryOperation<T>(this T operation, Action<T> action)
    {
        try
        {
            action(operation);
            return new Success();
        }
        catch (Exception e)
        {
            return new Failure(e);
        }
    }
}

