namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultImplicitTests
{
    [TestMethod]
    public void opImplicit_WithValue_ReturnsSuccess()
    {
        // arrange
        Success<string> result = "test";

        // act
        string value = result;

        // assert
        result.IsSuccess.Should().BeTrue();
        value.Should().Be("test");
    }

    [TestMethod]
    public void opImplicit_WithError_ReturnsFailure()
    {
        // arrange
        Failure<string> result = Error.Invalid("code", "test");

        // act

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public void opImplicit_WithErrors_ReturnsFailure()
    {
        // arrange
        Failure<string> result = new Error[]
        {
            Error.Invalid("code", "test"),
            Error.Validation("invalid", "test2")
        };

        // act

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public void opImplicit_WithException_ReturnsFailure()
    {
        // arrange
        Failure<string> result = new InvalidOperationException();

        // act

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }
}
