using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class ValidationErrorsAsyncTests
{
    [TestMethod]
    public async Task AddIfErrorAsync_WithError_AddsToList()
    {
        // arrange
        var errors = ValidationErrors.Create();

        // act
        await errors.AddIfErrorAsync(() => Task.FromResult(true), "Test.Error", "Test message");

        // assert
        errors.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task AddIfErrorAsync_WithNoError_KeepsListEmpty()
    {
        // arrange
        var errors = ValidationErrors.Create();

        // act
        await errors.AddIfErrorAsync(() => Task.FromResult(false), "Test.Error", "Test message");

        // assert
        errors.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task AddIfErrorAsync_WithErrorType_AddsToList()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();

        // act
        await errors.AddIfErrorAsync(() => Task.FromResult(true), err);

        // assert
        errors.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task AddIfError_WithNoErrorType_KeepsListEmpty()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();

        // act
        await errors.AddIfErrorAsync(() => Task.FromResult(false), err);

        // assert
        errors.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task MapAsync_WithError_ReturnsFailureResult()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => true, err);

        // act
        var result = await errors.MapAsync([ExcludeFromCodeCoverage] () => Task.FromResult(false));

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task MapAsync_WithoutError_CallsOnSuccess()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => false, err);

        // act
        var result = await errors.MapAsync(() => Task.FromResult(true));

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().BeTrue();
    }

    [TestMethod]
    public async Task AddIfErrorAsync_TaskWithError_AddsToList()
    {
        // arrange
        var errors = Task.FromResult(ValidationErrors.Create());

        // act
        var e = await errors.AddIfErrorAsync(() => Task.FromResult(true), "Test.Error", "Test message");

        // assert
        e.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task AddIfErrorAsync_TaskWithNoError_KeepsListEmpty()
    {
        // arrange
        var errors = Task.FromResult(ValidationErrors.Create());

        // act
        var e = await errors.AddIfErrorAsync(() => Task.FromResult(false), "Test.Error", "Test message");

        // assert
        e.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task AddIfErrorAsync_TaskWithErrorType_AddsToList()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = Task.FromResult(ValidationErrors.Create());

        // act
        var e = await errors.AddIfErrorAsync(() => Task.FromResult(true), err);

        // assert
        e.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task AddIfError_TaskWithNoErrorType_KeepsListEmpty()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = Task.FromResult(ValidationErrors.Create());

        // act
        var e = await errors.AddIfErrorAsync(() => Task.FromResult(false), err);

        // assert
        e.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task MapAsync_TaskWithError_ReturnsFailureResult()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => true, err);
        var task = Task.FromResult(errors);

        // act
        var result = await task.MapAsync([ExcludeFromCodeCoverage] () => Task.FromResult(false));

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task MapAsync_TaskWithoutError_CallsOnSuccess()
    {
        // arrange
        var err = Error.Validation("Test.Error", "Test message");
        var errors = ValidationErrors.Create();
        errors.AddIfError(() => false, err);
        var task = Task.FromResult(errors);

        // act
        var result = await task.MapAsync(() => Task.FromResult(true));

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().BeTrue();
    }
}
