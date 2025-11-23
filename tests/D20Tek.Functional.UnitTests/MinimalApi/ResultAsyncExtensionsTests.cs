using D20Tek.Functional.AspNetCore.MinimalApi.Async;

namespace D20Tek.Functional.UnitTests.MinimalApi.Async;

[TestClass]
public sealed class ResultAsyncExtensionsTests
{
    [TestMethod]
    public async Task ToApiResultAsync_WithNoParameter_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(101, "Test", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToApiResultAsync();

        // assert
        apiResult.ShouldBeOkResultValue(101, "Test");
    }

    [TestMethod]
    public async Task ToApiResultAsync_WithMappingFunc_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(101, "Test", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToApiResultAsync(ToResponse);

        // assert
        apiResult.ShouldBeOkResult(101, "Test");
    }

    [TestMethod]
    public async Task ToApiResultAsync_WithMappingFunc_ReturnsNotFound()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.NotFound);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToApiResultAsync(ToResponse);

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status404NotFound, "Not Found", error);
    }

    [TestMethod]
    public async Task ToApiResultAsync_WithResponse_ReturnsOK()
    {
        // arrange
        var entity = new TestEntity(101, "Test", DateTime.UtcNow);
        Result<TestEntity> modelResult = entity;
        var task = Task.FromResult(modelResult);

        // act
        var converted = ToResponse(entity);
        var apiResult = await task.ToApiResultAsync(converted);

        // assert
        apiResult.ShouldBeOkResult(101, "Test");
    }

    [TestMethod]
    public async Task ToApiResultAsync_WithResponse_ReturnsBadRequest()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Failure);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToApiResultAsync(new TestResponse(1, "foo"));

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status400BadRequest, "Bad Request", error);
    }

    [TestMethod]
    public async Task ToApiResultAsync_WithMultipleErrors_ReturnsFirstAndErrorList()
    {
        // arrange
        Error[] errors =
        [
            Error.Create("Test.Error", "Test error message", ErrorType.Unauthorized),
            Error.Validation("foo", "bar"),
            Error.Conflict("Test.Conflict", "Conflict error")
        ];

        var modelResult = Result<TestEntity>.Failure(errors);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToApiResultAsync();

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status401Unauthorized, "Unauthorized", errors);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithNoParameters_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync("CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeCreatedAtResultValue(201, "Test2");
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithNoParameters_ReturnsConflict()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Conflict);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync("CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", error);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithMappingFuncAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(ToResponse, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithMappingFuncAndRouteName_ReturnsConflict()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Conflict);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(ToResponse, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", error);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithResponseAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var converted = new TestResponse(201, "Test2");
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(converted, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithResponseAndRouteName_ReturnsUnprocessableEntity()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Invalid);
        var modelResult = Result<TestEntity>.Failure(error);
        var converted = new TestResponse(201, "Test2");
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(converted, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", error);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithOnlyRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync("/tests/301");

        // assert
        apiResult.ShouldBeCreatedResultValue("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithOnlyRouteUri_ReturnsUnexpected()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Unexpected);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync("/tests/301");

        // assert
        apiResult.ShouldBeProblemResult(
            StatusCodes.Status500InternalServerError,
            "An error occurred while processing your request.",
            error);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithMappingFuncAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(ToResponse, "/tests/301");

        // assert
        apiResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithMappingFuncAndRouteUri_ReturnsUnexpected()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Unexpected);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(ToResponse, "/tests/301");

        // assert
        apiResult.ShouldBeProblemResult(
            StatusCodes.Status500InternalServerError,
            "An error occurred while processing your request.",
            error);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithResponseAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var converted = new TestResponse(301, "Test3");
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(converted, "/tests/301");

        // assert
        apiResult.ShouldBeCreatedAtResult(301, "Test3");
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithResponseAndRouteUri_ReturnsForbidden()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Forbidden);
        var modelResult = Result<TestEntity>.Failure(error);
        var converted = new TestResponse(301, "Test3");
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedApiResultAsync(converted, "/tests/301");

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status403Forbidden, "Forbidden", error);
    }

    private TestResponse ToResponse(TestEntity entity) => new TestResponse(entity.Id, entity.Message);
}
