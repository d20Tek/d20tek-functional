using D20Tek.Functional.AspNetCore.WebApi;

namespace D20Tek.Functional.UnitTests.WebApi;

[TestClass]
public sealed class HandleResultFilterTests
{
    private readonly TestController _controller = new();
    private readonly List<IFilterMetadata> _filters = [];
    private readonly Dictionary<string, object?> _arguments = [];


    [TestMethod]
    public void OnActionExecuting_DoesNothing()
    {
        // arrange
        var result = new OkResult();
        var executingContext = CreateActionExecutingContext(result);
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuting(executingContext);

        // assert
        executingContext.Result.Should().Be(result);
    }

    [TestMethod]
    public void OnActionExecuted_WithNonResultType_ReturnsObjectResult()
    {
        // arrange
        var executedContext = CreateActionExecutedContext("Result.Success()");
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<ObjectResult>();
    }

    [TestMethod]
    public void TypedOnActionExecuted_WithSuccessResult_ReturnsOK()
    {
        // arrange
        var result = Result<TestResponse>.Success(new TestResponse(1, "two"));
        var executedContext = CreateActionExecutedContext(result);
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result!.ShouldBeOkResult(1, "two");
    }

    [TestMethod]
    public void TypedOnActionExecuted_WithSuccessNullResult_ReturnsOK()
    {
        // arrange
        var result = Result<TestResponse>.Success(null!);
        var executedContext = CreateActionExecutedContext(result);
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public void OnActionExecuted_WithFailureResult_ReturnsProblemDetails()
    {
        // arrange
        var result = Result<TestResponse>.Failure(Error.NotFound("notfound", "test"));
        var executedContext = CreateActionExecutedContext(result);

        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result!.ShouldBeProblemResult(StatusCodes.Status404NotFound, result.GetErrors().First());
    }

    [TestMethod]
    public void OnActionExecuted_WithOKResult_RemainsUnchanged()
    {
        // arrange
        var executedContext = CreateActionExecutedContext(new OkResult());
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public void OnActionExecuted_WithUnexpectedControllerType_RemainsUnchanged()
    {
        // arrange
        var result = Result<TestResponse>.Failure(Error.NotFound("notfound", "test"));
        var executedContext = CreateActionExecutedContext(result, "controller");

        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<ObjectResult>();
    }

    private ActionExecutedContext CreateActionExecutedContext(object result, object? controller = null) =>
        new(CreateActionContext(), _filters, controller ?? _controller)
        {
            Result = new ObjectResult(result)
        };

    private ActionExecutedContext CreateActionExecutedContext(IActionResult result) => 
        new(CreateActionContext(), _filters, _controller)
        {
            Result = result
        };

    private ActionExecutingContext CreateActionExecutingContext(IActionResult result) =>
        new(CreateActionContext(), _filters, _arguments, _controller)
        {
            Result = result
        };

    private static ActionContext CreateActionContext() =>
        new(new Mock<HttpContext>().Object, new RouteData(), new ActionDescriptor());
}
