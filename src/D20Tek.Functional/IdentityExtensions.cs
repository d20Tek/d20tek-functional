namespace D20Tek.Functional;

public static class IdentityExtensions
{
    public static Identity<T> ToIdentity<T>(this T source) where T : notnull => Identity<T>.Create(source);

    public static Identity<T> Flatten<T>(this Identity<Identity<T>> identity) where T : notnull => identity.Get();
}
