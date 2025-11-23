using D20Tek.Functional.AspNetCore.MinimalApi;

namespace D20Tek.Functional.UnitTests.MinimalApi;

[TestClass]
public sealed class ResultExtensionsTests
{
    [TestMethod]
    public void ToApiResult_WithNoParameter_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(101, "Test", DateTime.UtcNow);

        // act
        var apiResult = modelResult.ToApiResult();

        // assert
        apiResult.ShouldBeOkResultValue(101, "Test");
    }

    [TestMethod]
    public void ToApiResult_WithMappingFunc_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(101, "Test", DateTime.UtcNow);

        // act
        var apiResult = modelResult.ToApiResult(ToResponse);

        // assert
        apiResult.ShouldBeOkResult(101, "Test");
    }

    [TestMethod]
    public void ToApiResult_WithMappingFunc_ReturnsNotFound()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.NotFound);
        var modelResult = Result<TestEntity>.Failure(error);

        // act
        var apiResult = modelResult.ToApiResult(ToResponse);

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status404NotFound, "Not Found", error);
    }

    [TestMethod]
    public void ToApiResult_WithResponse_ReturnsOK()
    {
        // arrange
        var entity = new TestEntity(101, "Test", DateTime.UtcNow);
        Result<TestEntity> modelResult = entity;

        // act
        var converted = ToResponse(entity);
        var apiResult = modelResult.ToApiResult(converted);

        // assert
        apiResult.ShouldBeOkResult(101, "Test");
    }

    [TestMethod]
    public void ToApiResult_WithResponse_ReturnsBadRequest()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Failure);
        var modelResult = Result<TestEntity>.Failure(error);

        // act
        var apiResult = modelResult.ToApiResult(new TestResponse(1, "foo"));

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status400BadRequest, "Bad Request", error);
    }

    [TestMethod]
    public void ToApiResult_WithMultipleErrors_ReturnsFirstAndErrorList()
    {
        // arrange
        Error[] errors =
        [
            Error.Create("Test.Error", "Test error message", ErrorType.Unauthorized),
            Error.Validation("foo", "bar"),
            Error.Conflict("Test.Conflict", "Conflict error")
        ];

        var modelResult = Result<TestEntity>.Failure(errors);

        // act
        var apiResult = modelResult.ToApiResult();

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status401Unauthorized, "Unauthorized", errors);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithNoParameters_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);

        // act
        var apiResult = modelResult.ToCreatedApiResult("CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeCreatedAtResultValue(201, "Test2");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithNoParameters_ReturnsConflict()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Conflict);
        var modelResult = Result<TestEntity>.Failure(error);

        // act
        var apiResult = modelResult.ToCreatedApiResult("CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", error);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithMappingFuncAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);

        // act
        var apiResult = modelResult.ToCreatedApiResult(ToResponse, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithMappingFuncAndRouteName_ReturnsConflict()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Conflict);
        var modelResult = Result<TestEntity>.Failure(error);

        // act
        var apiResult = modelResult.ToCreatedApiResult(ToResponse, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", error);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithResponseAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var converted = new TestResponse(201, "Test2");

        // act
        var apiResult = modelResult.ToCreatedApiResult(converted, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithResponseAndRouteName_ReturnsUnprocessableEntity()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Invalid);
        var modelResult = Result<TestEntity>.Failure(error);
        var converted = new TestResponse(201, "Test2");

        // act
        var apiResult = modelResult.ToCreatedApiResult(converted, "CreateTest", new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", error);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithOnlyRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);

        // act
        var apiResult = modelResult.ToCreatedApiResult("/tests/301");

        // assert
        apiResult.ShouldBeCreatedResultValue("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithOnlyRouteUri_ReturnsUnexpected()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Unexpected);
        var modelResult = Result<TestEntity>.Failure(error);

        // act
        var apiResult = modelResult.ToCreatedApiResult("/tests/301");

        // assert
        apiResult.ShouldBeProblemResult(
            StatusCodes.Status500InternalServerError,
            "An error occurred while processing your request.",
            error);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithMappingFuncAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);

        // act
        var apiResult = modelResult.ToCreatedApiResult(ToResponse, "/tests/301");

        // assert
        apiResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithMappingFuncAndRouteUri_ReturnsUnexpected()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Unexpected);
        var modelResult = Result<TestEntity>.Failure(error);

        // act
        var apiResult = modelResult.ToCreatedApiResult(ToResponse, "/tests/301");

        // assert
        apiResult.ShouldBeProblemResult(
            StatusCodes.Status500InternalServerError,
            "An error occurred while processing your request.",
            error);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithResponseAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var converted = new TestResponse(301, "Test3");

        // act
        var apiResult = modelResult.ToCreatedApiResult(converted, "/tests/301");

        // assert
        apiResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithResponseAndRouteUri_ReturnsForbidden()
    {
        // arrange
        var error = Error.Create("Test.Error", "Test error message", ErrorType.Forbidden);
        var modelResult = Result<TestEntity>.Failure(error);
        var converted = new TestResponse(301, "Test3");

        // act
        var apiResult = modelResult.ToCreatedApiResult(converted, "/tests/301");

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status403Forbidden, "Forbidden", error);
    }

    private TestResponse ToResponse(TestEntity entity) => new TestResponse(entity.Id, entity.Message);
}
