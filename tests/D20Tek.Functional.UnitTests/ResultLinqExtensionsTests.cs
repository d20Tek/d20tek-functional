namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultLinqExtensionsTests
{
    [TestMethod]
    public void Select_WithSuccess_ReturnsProjectedSuccess()
    {
        // arrange
        var result = Result<int>.Success(21);

        // act
        var projected = result.Select(v => v * 2);

        // assert
        projected.IsSuccess.Should().BeTrue();
        projected.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Select_WithFailure_ReturnsFailure()
    {
        // arrange
        var result = Result<int>.Failure(Error.NotFound("id", "error"));

        // act
        var projected = result.Select([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        projected.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void Select_WithQuerySyntax_ProjectsValue()
    {
        // arrange
        var result = Result<int>.Success(10);

        // act
        var projected = from v in result
                        select v + 5;

        // assert
        projected.IsSuccess.Should().BeTrue();
        projected.GetValue().Should().Be(15);
    }

    [TestMethod]
    public void SelectMany_WithBothSuccess_ReturnsCombinedSuccess()
    {
        // arrange
        var result = Result<string>.Success("42");

        // act
        var combined = result.SelectMany(v => ResultHelper.TryParse(v), (text, number) => $"{text}:{number}");

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be("42:42");
    }

    [TestMethod]
    public void SelectMany_WithSourceFailure_ReturnsFailure()
    {
        // arrange
        var result = Result<string>.Failure(Error.NotFound("id", "error"));

        // act
        var combined = result.SelectMany(
            [ExcludeFromCodeCoverage] (v) => ResultHelper.TryParse(v),
            [ExcludeFromCodeCoverage] (text, number) => $"{text}:{number}");

        // assert
        combined.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void SelectMany_WithIntermediateFailure_ReturnsFailure()
    {
        // arrange
        var result = Result<string>.Success("non-int-text");

        // act
        var combined = result.SelectMany(
            v => ResultHelper.TryParse(v),
            [ExcludeFromCodeCoverage] (text, number) => $"{text}:{number}");

        // assert
        combined.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void SelectMany_WithQuerySyntax_ComposesTwoResults()
    {
        // arrange
        var first = Result<int>.Success(3);
        var second = Result<int>.Success(4);

        // act
        var combined = from a in first
                       from b in second
                       select a * b;

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(12);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void SelectMany_WithQuerySyntaxAndSecondFailure_ReturnsFailure()
    {
        // arrange
        var first = Result<int>.Success(3);
        var second = Result<int>.Failure(Error.Validation("second", "error"));

        // act
        var combined = from a in first
                       from b in second
                       select a * b;

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().ContainSingle(e => e.Code == "second");
    }

    [TestMethod]
    public void Where_WithSuccessSatisfyingPredicate_ReturnsSuccess()
    {
        // arrange
        var result = Result<int>.Success(42);
        var error = Error.Validation("Too.Small", "Value must be greater than 10.");

        // act
        var filtered = result.Where(v => v > 10, error);

        // assert
        filtered.IsSuccess.Should().BeTrue();
        filtered.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Where_WithSuccessFailingPredicate_ReturnsProvidedError()
    {
        // arrange
        var result = Result<int>.Success(5);
        var error = Error.Validation("Too.Small", "Value must be greater than 10.");

        // act
        var filtered = result.Where(v => v > 10, error);

        // assert
        filtered.IsFailure.Should().BeTrue();
        filtered.GetErrors().Should().ContainSingle(e => e.Code == "Too.Small");
    }

    [TestMethod]
    public void Where_WithFailure_ReturnsOriginalFailure()
    {
        // arrange
        var result = Result<int>.Failure(Error.NotFound("id", "error"));
        var error = Error.Validation("Too.Small", "Value must be greater than 10.");

        // act
        var filtered = result.Where([ExcludeFromCodeCoverage] (v) => v > 10, error);

        // assert
        filtered.IsFailure.Should().BeTrue();
        filtered.GetErrors().Should().ContainSingle(e => e.Code == "id");
    }
}
