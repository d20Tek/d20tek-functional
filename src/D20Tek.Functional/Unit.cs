namespace D20Tek.Functional;

/// <summary>
/// Represents the absence of a meaningful value—the functional equivalent of <c>void</c>.
/// Because C# generics cannot use <c>void</c> as a type argument, <see cref="Unit"/> allows
/// monadic types such as <see cref="Result{T}"/> to represent side-effect-only operations
/// (for example, <c>Result&lt;Unit&gt;</c>) without inventing a dummy value.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// Gets the singleton <see cref="Unit"/> value.
    /// </summary>
    public static Unit Value => default;

    /// <summary>
    /// Implicitly converts an empty <see cref="ValueTuple"/> into a <see cref="Unit"/>,
    /// enabling natural use of <c>default</c> and <c>()</c>-style expressions.
    /// </summary>
    /// <param name="_">The empty value tuple to convert.</param>
    public static implicit operator Unit(ValueTuple _) => default;

    /// <summary>
    /// Determines whether this instance is equal to another <see cref="Unit"/>.
    /// All <see cref="Unit"/> values are equal.
    /// </summary>
    /// <param name="other">The other <see cref="Unit"/> to compare against.</param>
    public bool Equals(Unit other) => true;

    /// <summary>
    /// Determines whether the specified object is a <see cref="Unit"/>.
    /// </summary>
    /// <param name="obj">The object to compare against.</param>
    public override bool Equals(object? obj) => obj is Unit;

    /// <summary>
    /// Returns a constant hash code, since all <see cref="Unit"/> values are equal.
    /// </summary>
    public override int GetHashCode() => 0;

    /// <summary>
    /// Determines whether two <see cref="Unit"/> values are equal. Always returns <c>true</c>.
    /// </summary>
    /// <param name="_">The first value.</param>
    /// <param name="__">The second value.</param>
    public static bool operator ==(Unit _, Unit __) => true;

    /// <summary>
    /// Determines whether two <see cref="Unit"/> values are unequal. Always returns <c>false</c>.
    /// </summary>
    /// <param name="_">The first value.</param>
    /// <param name="__">The second value.</param>
    public static bool operator !=(Unit _, Unit __) => false;

    /// <summary>
    /// Returns the string representation of <see cref="Unit"/>, following the F# convention <c>()</c>.
    /// </summary>
    public override string ToString() => "()";
}
