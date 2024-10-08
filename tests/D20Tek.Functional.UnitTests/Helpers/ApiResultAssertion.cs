﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace D20Tek.Functional.UnitTests.Helpers;

internal static class ApiResultAssertion
{
    public static void ShouldBeOkResult(
        this IResult apiResult,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Ok<TestResponse>>();

        var response = apiResult.As<Ok<TestResponse>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestResponse>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeOkResultValue(
        this IResult apiResult,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Ok<TestEntity>>();

        var response = apiResult.As<Ok<TestEntity>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestEntity>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedAtResult(
        this IResult apiResult,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<CreatedAtRoute<TestResponse>>();

        var response = apiResult.As<CreatedAtRoute<TestResponse>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestResponse>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedAtResultValue(
        this IResult apiResult,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<CreatedAtRoute<TestEntity>>();

        var response = apiResult.As<CreatedAtRoute<TestEntity>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestEntity>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedResult(
        this IResult apiResult,
        string expectedUri,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Created<TestResponse>>();

        var response = apiResult.As<Created<TestResponse>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Location.Should().Be(expectedUri);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestResponse>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedResultValue(
        this IResult apiResult,
        string expectedUri,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Created<TestEntity>>();

        var response = apiResult.As<Created<TestEntity>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Location.Should().Be(expectedUri);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestEntity>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeProblemResult(
        this IResult apiResult,
        int expectedStatusCode,
        string expectedTitle,
        Error? expectedError = null)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<ProblemHttpResult>();

        var response = apiResult.As<ProblemHttpResult>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(expectedStatusCode);
        response.ProblemDetails.Status.Should().Be(expectedStatusCode);
        response.ProblemDetails.Detail.Should().Be(expectedError?.Message);
        response.ProblemDetails.Title.Should().Be(expectedTitle);

        if (expectedError != null)
        {
            response.ProblemDetails.Extensions.Should().HaveCount(1);

            var ext = response.ProblemDetails.Extensions.First();
            ext.Key.Should().Be("errors");
            var errors = ext.Value as IDictionary<string, string>;
            errors.Should().NotBeNull();
            errors.Should().HaveCount(1);
            errors.Should().ContainKey(expectedError.Value.Code);
            errors.Should().ContainValue(expectedError.Value.Message);
        }
    }

    public static void ShouldBeProblemResult(
        this IResult apiResult,
        int expectedStatusCode,
        string expectedTitle,
        IEnumerable<Error> expectedErrors)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<ProblemHttpResult>();

        var response = apiResult.As<ProblemHttpResult>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(expectedStatusCode);
        response.ProblemDetails.Status.Should().Be(expectedStatusCode);
        response.ProblemDetails.Detail.Should().Be(expectedErrors.First().Message);
        response.ProblemDetails.Title.Should().Be(expectedTitle);
        response.ProblemDetails.Extensions.Should().HaveCount(1);

        var ext = response.ProblemDetails.Extensions.First();
        ext.Key.Should().Be("errors");
        var errors = ext.Value as IDictionary<string, string>;
        errors.Should().NotBeNull();
        errors.Should().HaveCount(expectedErrors.Count());
        foreach (var error in expectedErrors)
        {
            errors.Should().ContainKey(error.Code);
            errors.Should().ContainValue(error.Message);
        }
    }

    public static void ShouldBeValidationProblemResult(
        this IResult apiResult,
        int expectedStatusCode,
        string expectedTitle,
        IEnumerable<Error> expectedErrors)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<ProblemHttpResult>();

        var response = apiResult.As<ProblemHttpResult>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(expectedStatusCode);
        response.ProblemDetails.Status.Should().Be(expectedStatusCode);
        response.ProblemDetails.Title.Should().Be("One or more validation errors occurred.");

        var validations = response.ProblemDetails as HttpValidationProblemDetails;
        validations.Should().NotBeNull();
        validations!.Errors.Should().HaveCount(expectedErrors.Count());
        foreach (var error in expectedErrors)
        {
            validations.Errors.Should().ContainKey(error.Code);
        }
    }
}
