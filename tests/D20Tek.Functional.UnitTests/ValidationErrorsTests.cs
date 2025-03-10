namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ValidationErrorsTests
{
    [TestMethod]
    public void AddIfError_WithError_AddsToList()
    {
        // arrange
        var errors = ValidationErrors.Create();

        // act
        errors.AddIfError(() => true, "Test.Error", "Test message");

        // assert
        errors.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public void AddIfError_WithNoError_KeepsListEmpty()
    {
        // arrange
        var errors = ValidationErrors.Create();

        // act
        errors.AddIfError(() => false, "Test.Error", "Test message");

        // assert
        errors.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public void AddIfError_WithErrorType_AddsToList()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();

        // act
        errors.AddIfError(() => true, err);

        // assert
        errors.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public void AddIfError_WithNoErrorType_KeepsListEmpty()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();

        // act
        errors.AddIfError(() => false, err);

        // assert
        errors.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public void ToFailure_WithError_ReturnsFailureResult()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => true, err);

        // act
        var result = errors.ToFailure<Result<bool>>();

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void Map_WithError_ReturnsFailureResult()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => true, err);

        // act
        var result = errors.Map(() => false);

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public void Map_WithoutError_CallsOnSuccess()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => false, err);

        // act
        var result = errors.Map(() => true);

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().BeTrue();
    }
}
