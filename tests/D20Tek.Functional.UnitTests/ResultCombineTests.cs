namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultCombineTests
{
    private static readonly Error _error1 = Error.Validation("Test.Error1", "First error.");
    private static readonly Error _error2 = Error.Validation("Test.Error2", "Second error.");

    [TestMethod]
    public void Combine_WithAllSuccesses_ReturnsSuccessWithAllValues()
    {
        // arrange
        var results = new[] { Result<int>.Success(1), Result<int>.Success(2), Result<int>.Success(3) };

        // act
        var combined = Result.Combine(results);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Equal(1, 2, 3);
    }

    [TestMethod]
    public void Combine_WithEmptyArray_ReturnsSuccessWithEmptyValues()
    {
        // arrange

        // act
        var combined = Result.Combine<int>();

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().BeEmpty();
    }

    [TestMethod]
    public void Combine_WithMultipleFailures_CollectsAllErrors()
    {
        // arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(_error1),
            Result<int>.Failure(_error2),
        };

        // act
        var combined = Result.Combine(results);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Combine_WithNullResults_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(() => Result.Combine<int>(null!));
    }

    [TestMethod]
    public void Combine_WithTwoSuccesses_ProjectsCombinedValue()
    {
        // arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<string>.Success("a");

        // act
        var combined = Result.Combine(result1, result2, (a, b) => $"{a}{b}");

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be("1a");
    }

    [TestMethod]
    public void Combine_WithTwoResultsAndOneFailure_CollectsErrors()
    {
        // arrange
        var result1 = Result<int>.Failure(_error1);
        var result2 = Result<string>.Failure(_error2);

        // act
        var combined = Result.Combine(
            result1, result2, [ExcludeFromCodeCoverage] (a, b) => $"{a}{b}");

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Combine_WithTwoResultsAndNullCombiner_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Result.Combine(Result<int>.Success(1), Result<int>.Success(2), (Func<int, int, int>)null!));
    }

    [TestMethod]
    public void Combine_WithThreeSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = Result.Combine(
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3),
            (a, b, c) => a + b + c);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(6);
    }

    [TestMethod]
    public void Combine_WithThreeResultsAndFailure_CollectsErrors()
    {
        // arrange

        // act
        var combined = Result.Combine(
            Result<int>.Success(1),
            Result<int>.Failure(_error1),
            Result<int>.Failure(_error2),
            [ExcludeFromCodeCoverage] (a, b, c) => a + b + c);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Combine_WithThreeResultsAndNullCombiner_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Result.Combine(
                Result<int>.Success(1),
                Result<int>.Success(2),
                Result<int>.Success(3),
                (Func<int, int, int, int>)null!));
    }

    [TestMethod]
    public void Combine_WithFourSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = Result.Combine(
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3),
            Result<int>.Success(4),
            (a, b, c, d) => a + b + c + d);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(10);
    }

    [TestMethod]
    public void Combine_WithFourResultsAndFailure_CollectsErrors()
    {
        // arrange

        // act
        var combined = Result.Combine(
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Failure(_error1),
            Result<int>.Failure(_error2),
            [ExcludeFromCodeCoverage] (a, b, c, d) => a + b + c + d);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Combine_WithFourResultsAndNullCombiner_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Result.Combine(
                Result<int>.Success(1),
                Result<int>.Success(2),
                Result<int>.Success(3),
                Result<int>.Success(4),
                (Func<int, int, int, int, int>)null!));
    }

    [TestMethod]
    public void Combine_WithFiveSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = Result.Combine(
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3),
            Result<int>.Success(4),
            Result<int>.Success(5),
            (a, b, c, d, e) => a + b + c + d + e);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(15);
    }

    [TestMethod]
    public void Combine_WithFiveResultsAndFailure_CollectsErrors()
    {
        // arrange

        // act
        var combined = Result.Combine(
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3),
            Result<int>.Failure(_error1),
            Result<int>.Failure(_error2),
            [ExcludeFromCodeCoverage] (a, b, c, d, e) => a + b + c + d + e);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Combine_WithFiveResultsAndNullCombiner_ThrowsArgumentNullException()
    {
        // act and assert
        Assert.ThrowsExactly<ArgumentNullException>(
            () => Result.Combine(
                Result<int>.Success(1),
                Result<int>.Success(2),
                Result<int>.Success(3),
                Result<int>.Success(4),
                Result<int>.Success(5),
                (Func<int, int, int, int, int, int>)null!));
    }
}
