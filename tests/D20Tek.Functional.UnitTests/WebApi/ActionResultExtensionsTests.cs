﻿using D20Tek.Functional.AspNetCore.WebApi;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Functional.UnitTests.WebApi;

[TestClass]
public sealed class ActionResultExtensionsTests
{
    private readonly TestController _controller = new();

    [TestMethod]
    public void Problem_WithEmptyErrors_ShouldProduceDefaultProblemDetails()
    {
        // arrange
        var errors = new List<Error>();

        // act
        var result = _controller.Problem<TestResponse>(errors);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status500InternalServerError);
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
        var result = _controller.Problem<TestResponse>(errors);

        // assert
        result.ShouldBeValidationProblemResult(errors);
    }

    [TestMethod]
    public void Problem_WithSingleError_ShouldProduceProblemDetails()
    {
        // arrange
        var error = Error.Conflict("conflict", "test");

        // act
        var result = _controller.Problem<TestResponse>(error);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status409Conflict, error);
    }

    [TestMethod]
    public void Problem_WithSingleValidationError_ShouldProduceProblemDetails()
    {
        // arrange
        var error = Error.Validation("Test.Name.Empty", "Name is required.");

        // act
        var result = _controller.Problem<TestResponse>(error);

        // assert
        result.ShouldBeValidationProblemResult(new List<Error> { error });
    }

    [TestMethod]
    public void Problem_WithDecomposedError_ShouldProduceProblemDetails()
    {
        // arrange
        var expectedError = Error.Create("Custom.Code", "Custom test error.", StatusCodes.Status405MethodNotAllowed);

        // act
        var result = _controller.Problem<TestResponse>(
            StatusCodes.Status405MethodNotAllowed,
            "Custom.Code",
            "Custom test error.");

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status405MethodNotAllowed, expectedError);
    }

    [TestMethod]
    public void Problem_WithMixedValidationErrors_ShouldProduceProblemDetailsWithOneItem()
    {
        // arrange
        var errors = new List<Error>
        {
            Error.Validation("Test.Id.Missing", "Id error."),
            Error.Unexpected("unexpected", "test")
        };

        // act
        var result = _controller.Problem<TestResponse>(errors);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status400BadRequest, errors);
    }

    [TestMethod]
    public void Problem_WithProblemDetailsFactory_ShouldProduceProblemDetails()
    {
        // arrange
        var error = Error.Conflict("conflict", "test");
        _controller.ProblemDetailsFactory = new TestProblemDetailsFactory();

        // act
        var result = _controller.Problem<TestResponse>(error);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status409Conflict, error);
    }

    [TestMethod]
    public void Problem_WithProblemDetailsFactory_ShouldProduceValidationProblemDetails()
    {
        // arrange
        var error = Error.Validation("Test.Name.Empty", "Name is required.");
        _controller.ProblemDetailsFactory = new TestProblemDetailsFactory();

        // act
        var result = _controller.Problem<TestResponse>(error);

        // assert
        result.ShouldBeValidationProblemResult(new List<Error> { error });
    }
}
