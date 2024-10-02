namespace D20Tek.Functional.UnitTests;

[TestClass]
public class TryExceptTests
{
    [TestMethod]
    public void Run_WithSuccessfulOperation_ReturnsSuccess()
    {
        // arrange

        // act
        var result = TryExcept.Run(
            () => Result<int>.Success(42),
            [ExcludeFromCodeCoverage] (ex) => Result<int>.Failure(ex));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Run_WithOperationException_ReturnsFailure()
    {
        // arrange

        // act
        var result = TryExcept.Run(
            () => throw new InvalidOperationException(),
            ex => Result<int>.Failure(ex));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }
}
