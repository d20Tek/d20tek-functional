namespace D20Tek.Functional.AspNetCore;

/// <summary>
/// Maps <see cref="ErrorType"/> integer values to <see cref="System.Net.HttpStatusCode"/> values.
/// Used by the ASP.NET Core integration to convert domain errors into appropriate HTTP responses.
/// Access the singleton instance via <see cref="ErrorTypeMapper.Instance"/>.
/// </summary>
public interface IErrorTypeMapper
{
    /// <summary>
    /// Reconfigures the mapper using the provided configurator action.
    /// Passing <c>null</c> resets to default mappings.
    /// </summary>
    /// <param name="configure">An optional action to customize the mappings.</param>
    IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null);

    /// <summary>
    /// Returns whether a mapping exists for the specified error type.
    /// </summary>
    /// <param name="errorType">The numeric error type to check.</param>
    bool Contains(int errorType);

    /// <summary>
    /// Converts an error type to its mapped HTTP status code.
    /// Returns <see cref="System.Net.HttpStatusCode.InternalServerError"/> if no mapping exists.
    /// </summary>
    /// <param name="errorType">The numeric error type to convert.</param>
    HttpStatusCode Convert(int errorType);

    /// <summary>
    /// Adds or overrides a mapping from an error type to an HTTP status code.
    /// </summary>
    /// <param name="errorType">The numeric error type.</param>
    /// <param name="statusCode">The HTTP status code to map to.</param>
    IErrorTypeMapper For(int errorType, HttpStatusCode statusCode);

    /// <summary>
    /// Removes the mapping for the specified error type.
    /// </summary>
    /// <param name="errorType">The numeric error type to remove.</param>
    IErrorTypeMapper Remove(int errorType);
}
