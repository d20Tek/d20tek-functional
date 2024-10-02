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
}
