using D20Tek.Functional.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace D20Tek.Functional.UnitTests.MinimalApi;

[TestClass]
public sealed class HandleResultFilterTests
{
    private readonly Mock<EndpointFilterInvocationContext> _mockContext = new();

    [TestMethod]
    public async Task OnActionExecuted_WithNonResultType_ReturnsObjectResult()
    {
        // arrange
        var result = "Result.Success()";
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(_mockContext.Object, ctx => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<ObjectResult>();
    }

    [TestMethod]
    public async Task TypedOnActionExecuted_WithSuccessResult_ReturnsOK()
    {
        // arrange
        var result = Result<TestResponse>.Success(new TestResponse(1, "two"));
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(_mockContext.Object, ctx => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<Ok<object>>();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);

        var response = res.Value.As<TestResponse>();
        response.Should().NotBeNull();
        response.Id.Should().Be(1);
        response.Message.Should().Be("two");
    }

    [TestMethod]
    public async Task OnActionExecuted_WithFailureResult_ReturnsProblemDetails()
    {
        // arrange
        var result = Result<TestResponse>.Failure(Error.Conflict("conflict", "test"));
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(_mockContext.Object, ctx => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<ProblemHttpResult>();
        res.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", result.GetErrors().First());
    }

    [TestMethod]
    public async Task OnActionExecuted_WithOKResult_RemainsUnchanged()
    {
        // arrange
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(_mockContext.Object, ctx => new ValueTask<object?>(new OkResult()));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<OkResult>();
    }

    [TestMethod]
    public async Task OnActionExecuted_WithSuccessNullResult_ReturnsOK()
    {
        // arrange
        var result = Result<TestResponse>.Success(null!);
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(_mockContext.Object, ctx => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<Ok>();
        res.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
