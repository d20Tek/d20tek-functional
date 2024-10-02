namespace D20Tek.Functional;

public static class BooleanExtensions
{
    public static TOut IfTrueOrElse<TOut>(this bool condition, Func<TOut> thenFunc, Func<TOut> elseFunc) =>
        condition ? thenFunc() : elseFunc();

    public static void IfTrueOrElse(this bool condition, Action thenAction, Action? elseAction = null)
    {
        if (condition)
            thenAction();
        else
            elseAction?.Invoke();
    }
}
