namespace D20Tek.Functional.UnitTests;

[TestClass]
public class FunctionalExtensionsTests
{
    [TestMethod]
    public void Fork_WithTwoInputFuncs_ReturnsCombinedOutput()
    {
        // arrange
        double[] grades = [4, 3.5, 3.7, 3.3, 4.3];

        // act
        var result = grades.Fork(
            g => g.Sum(),
            g => g.Count(),
            (totalPoints, numClasses) => Math.Round(totalPoints / numClasses, 2));

        result.Should().Be(3.76);
    }

    [TestMethod]
    public void IterateUntil_RunUpdateFunction_UntilEndCondition()
    {
        // arrange
        var count = 0;

        // act
        var result = count.IterateUntil(
            x => x += 1,
            x => x > 5);

        // assert
        result.Should().Be(6);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(DivideByZeroException))]
    public void IterateUntil_UpdateThrowsException_LogsError()
    {
        // arrange
        var count = 0;

        // act
        var result = count.IterateUntil(
            x => x / 0,
            x => x > 5);

        // assert
    }

    [TestMethod]
    public void IterateUntil_RunUpdateResultFunction_UntilEndCondition()
    {
        // arrange
        var count = 0;

        // act
        var result = count.IterateUntil(
            x => Result<int>.Success(x += 1),
            x => x > 5);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(6);
    }

    [TestMethod]
    public void IterateUntil_RunUpdateFunction_ReturnsSuccess()
    {
        // arrange
        var count = 0;

        // act
        var result = count.IterateUntil(
            x => Result<int>.Failure(Error.Unexpected("code", "text")),
            x => x > 5);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public void IterateUntil_RunThrowsException_ReturnsF()
    {
        // arrange
        var count = 0;

        // act
        var result = count.IterateUntil(
            [ExcludeFromCodeCoverage] (x) => Result<int>.Success(x / 0),
            x => x > 5);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Alt_WithTwoInputFuncs_ReturnsThroughAll()
    {
        // arrange
        var text = "foo";

        // act
        var result = text.Alt(
            x => x == "bar" ? "it was bar" : null,
            x => x == "foo" ? "it was foo" : null);

        // assert
        result.Should().Be("it was foo");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Alt_WithTwoInputFuncs_ReturnsFirstPath()
    {
        // arrange
        var text = "bar";

        // act
        var result = text.Alt(
            x => x == "bar" ? "it was bar" : null,
            x => x == "foo" ? "it was foo" : null);

        // assert
        result.Should().Be("it was bar");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void Alt_WithNoMatches_ReturnsNull()
    {
        // arrange
        var text = "other";

        // act
        var result = text.Alt(
            x => x == "bar" ? "it was bar" : null,
            x => x == "foo" ? "it was foo" : null);

        // assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void Pipe_WithFunc_ReturnsChainedResult()
    {
        // arrange
        Func<int, int, int> add = (x, y) => x + y;

        // act
        var result = add(1, 3).Pipe(z => add(z, 5));

        // assert
        result.Should().Be(9);
    }

    [TestMethod]
    public void Pipe_WithFunc_ReturnsChainedResult2()
    {
        // arrange
        Func<string, string, string> add = (x, y) => $"{x} {y}";

        // act
        var result = add("test", "with").Pipe(z => add(z, "string"));

        // assert
        result.Should().Be("test with string");
    }

    [TestMethod]
    public void Pipe_WithAction_ReturnsChainedResult2()
    {
        // arrange
        var output = "test";
        Action<string> append = (x) => output += $" {x}";

        // act
        var result = output.Pipe(_ => append("with")).Pipe(_ => append("string"));

        // assert
        output.Should().Be("test with string");
    }
}
