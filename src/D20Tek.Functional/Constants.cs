namespace D20Tek.Functional;

internal static class Constants
{
    public static ArgumentOutOfRangeException ChoiceValueException = new("value", "Invalid Choice type.");
    public static ArgumentNullException ValueNullException = new("Value");
    public const string GeneralExceptionCode = "General.Exception";
    public const string ListSeparator = ", ";
    public static Error ResultFilterError = Error.NotFound("Filter.Error", "No filtered items found.");

    public static string ChoiceFormatString(Type type, object value) => $"Choice<{type.Name}>(value = {value})";

    public static string ChoiceAsyncFormatString(Type type, object value) => $"ChoiceAsync<{type.Name}>(value = {value})";

    public static string IdentityFormatString(Type type, object value) => $"Identity<{type.Name}>(value = {value})";

    public static string SomeFormatString(Type type, object value) => $"Some<{type.Name}>(value = {value})";

    public static string NoneFormatString(Type type) => $"None<{type.Name}>";

    public static string SuccessFormatString(Type type, object value) => $"Success<{type.Name}>(value = {value})";

    public static string FailureFormatString(Type type, Error[] errors) =>
        $"Failure<{type.Name}>(errors = {string.Join(Environment.NewLine, errors)})";
}
