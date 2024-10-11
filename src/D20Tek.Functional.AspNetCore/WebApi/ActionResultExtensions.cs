using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace D20Tek.Functional.AspNetCore.WebApi;

public static class ActionResultExtensions
{
    private const string _errorsExtensionName = "errors";

    public static ActionResult<TResult> Problem<TResult>(this ControllerBase controller, IEnumerable<Error> errors) =>
        errors.Any() && errors.All(e => e.Type == ErrorType.Validation) 
            ? ValidationProblem<TResult>(controller, errors)
            : ProblemInternal<TResult>(controller, errors);

    public static ActionResult<TResult> Problem<TResult>(this ControllerBase controller, Error error) =>
        (error.Type == ErrorType.Validation)
            ? ValidationProblem<TResult>(controller,[error])
            : controller.Problem(
                statusCode: MapErrorToCode(error),
                detail: error.Message,
                errorsExtension: CreateErrorsExtension(error));

    public static ActionResult<TResult> Problem<TResult>(
        this ControllerBase controller,
        int statusCode,
        string errorCode,
        string message) =>
        controller.Problem(
            statusCode: statusCode,
            detail: message,
            errorsExtension: CreateErrorsExtension(Error.Create(errorCode, message, statusCode)));

    public static IActionResult ToIActionResult(this IConvertToActionResult actionResult) => actionResult.Convert();

    private static ActionResult<TResult> ProblemInternal<TResult>(ControllerBase controller, IEnumerable<Error> errors) =>
        errors.Any()
            ? errors.First().Pipe(error => controller.Problem(
                statusCode: MapErrorToCode(error), detail: error.Message, errorsExtension: CreateErrorsExtension(errors)))
            : controller.Problem();

    private static int MapErrorToCode(Error error) => (int)ErrorTypeMapper.Instance.Convert(error.Type);

    private static Dictionary<string, string> CreateErrorsExtension(IEnumerable<Error> errors) =>
        errors.ToDictionary(error => error.Code, error => error.Message);

    private static Dictionary<string, string> CreateErrorsExtension(Error error) =>
        new()
        {
            { error.Code, error.Message }
        };

    private static ObjectResult Problem(
        this ControllerBase controller,
        string? detail = null,
        string? instance = null,
        int? statusCode = StatusCodes.Status500InternalServerError,
        string? title = null,
        string? type = null,
        IDictionary<string, string>? errorsExtension = null) =>
        CreateProblemDetails(controller, detail, instance, statusCode, title, type)
            .Iter(details => details.Extensions.Add(_errorsExtensionName, errorsExtension))
            .Map(details => new ObjectResult(details)
            {
                StatusCode = details.Status
            });

    private static Identity<ProblemDetails> CreateProblemDetails(
        ControllerBase controller,
        string? detail,
        string? instance,
        int? statusCode,
        string? title,
        string? type) =>
        controller.ProblemDetailsFactory == null
                ? new ProblemDetails
                {
                    Detail = detail,
                    Instance = instance,
                    Status = statusCode,
                    Title = title,
                    Type = type,
                }
                : controller.ProblemDetailsFactory.CreateProblemDetails(
                    controller.HttpContext,
                    statusCode: statusCode,
                    title: title,
                    type: type,
                    detail: detail,
                    instance: instance);

    private static ActionResult<TValue> ValidationProblem<TValue>(
        ControllerBase controller,
        IEnumerable<Error> errors) =>
        controller.ValidationProblem(
            errors.Aggregate(
                new ModelStateDictionary(),
                (acc, error) =>
                {
                    acc.AddModelError(error.Code, error.Message);
                    return acc;
                }));
}
