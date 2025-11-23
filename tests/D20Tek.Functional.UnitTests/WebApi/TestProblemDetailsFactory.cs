namespace D20Tek.Functional.UnitTests.WebApi;

internal sealed class TestProblemDetailsFactory : ProblemDetailsFactory
{
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = StatusCodes.Status500InternalServerError,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance,
        };
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = StatusCodes.Status400BadRequest,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        return problemDetails;
    }
}
