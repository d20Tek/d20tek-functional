namespace D20Tek.Functional;

/// <summary>
/// A non-generic interface exposing the success/failure state of a <see cref="Result{T}"/>.
/// Used by infrastructure code (such as ASP.NET Core endpoint filters) that needs to inspect
/// result outcomes without knowing the concrete value type at compile time.
/// </summary>
public interface IResultMonad
{
    /// <summary>
    /// Gets the success value as an <see cref="object"/>, or <c>null</c> if the result is a failure.
    /// </summary>
    public object? GetValue();

    /// <summary>
    /// Gets the array of <see cref="Error"/> instances associated with this result.
    /// Returns an empty array when the result is successful.
    /// </summary>
    public Error[] GetErrors();

    /// <summary>Gets whether this result represents a successful outcome.</summary>
    public bool IsSuccess { get; }

    /// <summary>Gets whether this result represents a failed outcome.</summary>
    public bool IsFailure { get; }
}
