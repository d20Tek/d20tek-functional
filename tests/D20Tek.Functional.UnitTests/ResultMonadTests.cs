namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultMonadTests
{
    [TestMethod]
    public void GetValue_WithSuccess_ReturnsValue()
    {
        // arrange
        var response = new TestResponse(1, "test");
        IResultMonad result = Result<TestResponse>.Success(response);

        // act
        var value = result.GetValue();

        // assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        value.Should().NotBeNull();
        value.Should().Be(response);
    }

    [TestMethod]
    public void GetValue_WithSuccessButNullValue_ReturnsNull()
    {
        // arrange
        IResultMonad result = Result<TestResponse>.Success(null!);

        // act
        var value = result.GetValue();

        // assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        value.Should().BeNull();
    }

    [TestMethod]
    public void GetValue_WithError_ReturnsNull()
    {
        // arrange
        var error = Error.Validation("invalid", "test");
        IResultMonad result = Result<TestResponse>.Failure(error);

        // act
        var value = result.GetValue();

        // assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        value.Should().BeNull();
    }

    [TestMethod]
    public void GetErrors_WithError_ReturnsErrorArray()
    {
        // arrange
        var error = Error.Validation("invalid", "test");
        IResultMonad result = Result<TestResponse>.Failure(error);

        // act
        var errors = result.GetErrors();

        // assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        errors.Should().NotBeNull();
        errors.Should().NotBeEmpty();
    }

    [TestMethod]
    public void GetErrors_WithSuccess_ReturnsEmptyErrorArray()
    {
        // arrange
        var response = new TestResponse(1, "test");
        IResultMonad result = Result<TestResponse>.Success(response);

        // act
        var errors = result.GetErrors();

        // assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        errors.Should().NotBeNull();
        errors.Should().BeEmpty();
    }
}
