using D20Tek.Functional.AspNetCore.MinimalApi;

namespace D20Tek.Functional.UnitTests.MinimalApi;

[TestClass]
public sealed class TypedResultExtensionsTests
{
    [TestMethod]
    public void Problem_WithEmptyErrors_ShouldProduceDefaultProblemDetails()
    {
        // arrange
        var errors = new List<Error>();

        // act
        var result = Results.Extensions.Problem(errors);

        // assert
        result.ShouldBeProblemResult(
            StatusCodes.Status500InternalServerError,
            "An error occurred while processing your request.");
    }

    [TestMethod]
    public void Problem_WithValidationErrors_ShouldProduceProblemDetailsWithMultipleItems()
    {
        // arrange
        var errors = new List<Error>
        {
            Error.Validation("Test.Id.Missing", "Id error."),
            Error.Validation("Test.Name.Empty", "Name is required.")
        };

        // act
        var result = Results.Extensions.Problem(errors);

        // assert
        result.ShouldBeValidationProblemResult(StatusCodes.Status400BadRequest, "Bad Request", errors);
    }

    [TestMethod]
    public void Problem_WithSingleError_ShouldProduceProblemDetails()
    {
        // arrange
        var error = Error.Conflict("Conflict", "Conflict error");

        // act
        var result = Results.Extensions.Problem(error);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", error);
    }

    [TestMethod]
    public void Problem_WithSingleValidationError_ShouldProduceProblemDetails()
    {
        // arrange
        var error = Error.Validation("Test.Name.Empty", "Name is required.");

        // act
        var result = Results.Extensions.Problem(error);

        // assert
        result.ShouldBeValidationProblemResult(StatusCodes.Status400BadRequest, "Bad Request", [error]);
    }

    [TestMethod]
    public void Problem_WithDecomposedError_ShouldProduceProblemDetails()
    {
        // arrange
        var expectedError = Error.Create("Custom.Code", "Custom test error.", StatusCodes.Status405MethodNotAllowed);

        // act
        var result = Results.Extensions.Problem(
            StatusCodes.Status405MethodNotAllowed,
            "Custom.Code",
            "Custom test error.");

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status405MethodNotAllowed, "Method Not Allowed", expectedError);
    }

    [TestMethod]
    public void Problem_WithMixedValidationErrors_ShouldProduceProblemDetailsWithOneItem()
    {
        // arrange
        var errors = new List<Error>
        {
            Error.Validation("Test.Id.Missing", "Id error."),
            Error.Unexpected("Unexpected.Error", "test error.")
        };

        // act
        var result = Results.Extensions.Problem(errors);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status400BadRequest, "Bad Request", errors);
    }
}
