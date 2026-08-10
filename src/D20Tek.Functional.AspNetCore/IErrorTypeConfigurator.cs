namespace D20Tek.Functional.AspNetCore;

/// <summary>
/// Fluent interface for configuring the mapping between <see cref="ErrorType"/> integer values
/// and <see cref="System.Net.HttpStatusCode"/> responses. Used within <see cref="IErrorTypeMapper.Configure"/>.
/// </summary>
public interface IErrorTypeConfigurator
{
    /// <summary>
    /// Removes all existing error-type-to-status-code mappings.
    /// </summary>
    IErrorTypeConfigurator Clear();

    /// <summary>
    /// Maps (or overrides) an error type to a specific HTTP status code.
    /// </summary>
    /// <param name="errorType">The numeric error type (see <see cref="ErrorType"/> constants).</param>
    /// <param name="statusCode">The HTTP status code to map to.</param>
    IErrorTypeConfigurator For(int errorType, HttpStatusCode statusCode);

    /// <summary>
    /// Removes the mapping for the specified error type.
    /// </summary>
    /// <param name="errorType">The numeric error type to remove.</param>
    IErrorTypeConfigurator Remove(int errorType);
}