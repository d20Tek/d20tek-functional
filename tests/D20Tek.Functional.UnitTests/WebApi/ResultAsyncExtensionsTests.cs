using D20Tek.Functional.AspNetCore.WebApi;
using D20Tek.Functional.AspNetCore.WebApi.Async;

namespace D20Tek.Functional.UnitTests.WebApi.Async;

[TestClass]
public sealed class ResultAsyncExtensionsTests
{
    private readonly TestController _controller = new TestController();

    [TestMethod]
    public async Task ToActionResultAsync_WithMappingFunc_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(42, "ActionTest", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToActionResultAsync(ToResponse, _controller);

        // assert
        actionResult.ShouldBeOkResult(42, "ActionTest");
    }

    [TestMethod]
    public async Task ToActionResultAsync_WithMappingFunc_ReturnsNotFound()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.NotFound);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToActionResultAsync(ToResponse, _controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status404NotFound, error);
    }

    [TestMethod]
    public async Task ToActionResult_WithResponse_ReturnsOK()
    {
        // arrange
        var entity = new TestEntity(101, "Test", DateTime.UtcNow);
        Result<TestEntity> modelResult = entity;
        var task = Task.FromResult(modelResult);

        // act
        var converted = ToResponse(entity);
        var actionResult = await task.ToActionResultAsync(converted, _controller);

        // assert
        actionResult.ShouldBeOkResult(101, "Test");
    }

    [TestMethod]
    public async Task ToActionResultAsync_WithResponse_ReturnsBadRequest()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Failure);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToActionResultAsync(new TestResponse(1, "foo"), _controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status400BadRequest, error);
    }

    [TestMethod]
    public async Task ToActionResultAsync_WithMultipleErrors_ReturnsFirstAndErrorList()
    {
        // arrange
        Error[] errors =
        [
            Error.Create("Test.Error", "Test error message", ErrorType.Unauthorized),
            Error.Validation("foo", "bar"),
            Error.Conflict("conflict", "test")
        ];
        var modelResult = Result<TestEntity>.Failure(errors);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToActionResultAsync(ToResponse, _controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status401Unauthorized, errors);
    }

    [TestMethod]
    public async Task ToCreatedActionResultAsync_WithMappingFuncAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToCreatedActionResultAsync(
            ToResponse, _controller, "CreateTest", new { id = 201 });

        // assert
        actionResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public async Task ToCreatedActionResultAsync_WithMappingFuncAndRouteName_ReturnsConflict()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Conflict);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var apiResult = await task.ToCreatedActionResultAsync(
            ToResponse, _controller, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status409Conflict, error);
    }

    [TestMethod]
    public async Task ToCreatedApiResultAsync_WithResponseAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var converted = new TestResponse(201, "Test2");
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToCreatedActionResultAsync(
            converted, _controller, "CreateTest", new { id = 201 });

        // assert
        actionResult.ShouldBeCreatedAtResult(201, "Test2");
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
        var actionResult = await task.ToCreatedActionResultAsync(
            converted, _controller, "CreateTest", new { id = 201 });

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status422UnprocessableEntity, error);
    }

    [TestMethod]
    public async Task ToCreatedActionResultAsync_WithMappingFuncAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToCreatedActionResultAsync(ToResponse, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public async Task ToCreatedActionResultAsync_WithMappingFuncAndRouteUri_ReturnsUnexpected()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Unexpected);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToCreatedActionResultAsync(ToResponse, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status500InternalServerError, error);
    }

    [TestMethod]
    public async Task ToCreatedActionResultAsync_WithResponseAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var converted = new TestResponse(301, "Test3");
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToCreatedActionResultAsync(converted, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public async Task ToCreatedActionResultAsync_WithResponseAndRouteUri_ReturnsForbidden()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Forbidden);
        var modelResult = Result<TestEntity>.Failure(error);
        var converted = new TestResponse(301, "Test3");
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToCreatedActionResultAsync(converted, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status403Forbidden, error);
    }

    [TestMethod]
    public async Task ToIActionResultAsync_WithMappingFunc_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(42, "ActionTest", DateTime.UtcNow);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToActionResultAsync(ToResponse, _controller).ToIActionResultAsync();

        // assert
        actionResult.ShouldBeOkResult(42, "ActionTest");
    }

    [TestMethod]
    public async Task ToIActionResult_WithMappingFunc_ReturnsNotFound()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.NotFound);
        var modelResult = Result<TestEntity>.Failure(error);
        var task = Task.FromResult(modelResult);

        // act
        var actionResult = await task.ToActionResultAsync(ToResponse, _controller).ToIActionResultAsync();

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status404NotFound, error);
    }

    private TestResponse ToResponse(TestEntity entity) => new(entity.Id, entity.Message);
}
