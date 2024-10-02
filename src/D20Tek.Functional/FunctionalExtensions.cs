namespace D20Tek.Functional;

public static class FunctionalExtensions
{
    public static TOut Fork<TIn, T1, T2, TOut>(
        this TIn instance,
        Func<TIn, T1> f1,
        Func<TIn, T2> f2,
        Func<T1, T2, TOut> fOut) =>
        fOut(f1(instance), f2(instance));

    public static T IterateUntil<T>(this T instance, Func<T, T> updateFunction, Func<T, bool> endCondition)
    {
        var currentThis = instance;

        try
        {
            while (!endCondition(currentThis))
            {
                currentThis = updateFunction(currentThis);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            throw;
        }

        return currentThis;
    }
}
