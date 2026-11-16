namespace D20Tek.Functional.AspNetCore;

internal static class Constants
{
    public const string RouteValuesTrimMessage =
        "This method uses ASP.NET Core CreatedAtRoute, which reflects over the routeValues object to build the " +
        "route. The public properties of the routeValues type may be trimmed. Use an overload that accepts a " +
        "route URI, or pass a RouteValueDictionary for routeValues.";
}
