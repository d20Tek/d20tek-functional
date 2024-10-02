namespace D20Tek.Functional;

public static class TryExcept
{
    public static T Run<T>(Func<T> operation, Func<Exception, T> onException)
        where T : notnull
    {
        try
        {
            return operation();
        }
        catch (Exception e)
        {
            return onException(e);
        }
    }
}
