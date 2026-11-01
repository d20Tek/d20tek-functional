namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultTraverseTests
{
    private static readonly Error _error1 = Error.Validation("Test.Error1", "First error.");
    private static readonly Error _error2 = Error.Validation("Test.Error2", "Second error.");

    [TestMethod]
    public void Traverse_WithAllSuccesses_ReturnsSuccessWithMappedValues()
    {
        // arrange
        var source = new[] { 1, 2, 3 };

        // act
        var result = Result.Traverse(source, x => Result<int>.Success(x * 2));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Equal(2, 4, 6);
    }

    [TestMethod]
    public void Traverse_WithEmptySource_ReturnsSuccessWithEmptyValues()
    {
        // arrange
        var source = Array.Empty<int>();

        // act
        var result = Result.Traverse(source, [ExcludeFromCodeCoverage] (x) => Result<int>.Success(x));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().BeEmpty();
    }

    [TestMethod]
    public void Traverse_WithSomeFailures_CollectsAllErrors()
    {
        // arrange
        var source = new[] { 1, 2, 3 };

        // act
        var result = Result.Traverse(
            source,
            x => x switch
            {
                2 => Result<int>.Failure(_error1),
                3 => Result<int>.Failure(_error2),
                _ => Result<int>.Success(x),
            });

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Traverse_WithNullSource_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Result.Traverse(null!, (int x) => Result<int>.Success(x)));
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Traverse_WithNullSelector_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Result.Traverse<int, int>([1, 2], null!));
    }

    [TestMethod]
    public void Sequence_WithAllSuccesses_ReturnsSuccessWithAllValues()
    {
        // arrange
        var source = new List<Result<int>> { Result<int>.Success(1), Result<int>.Success(2) };

        // act
        var result = Result.Sequence(source);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Equal(1, 2);
    }

    [TestMethod]
    public void Sequence_WithFailures_CollectsAllErrors()
    {
        // arrange
        var source = new List<Result<int>>
        {
            Result<int>.Failure(_error1),
            Result<int>.Success(2),
            Result<int>.Failure(_error2),
        };

        // act
        var result = Result.Sequence(source);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Sequence_WithNullSource_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(() => Result.Sequence<int>(null!));
    }
}
