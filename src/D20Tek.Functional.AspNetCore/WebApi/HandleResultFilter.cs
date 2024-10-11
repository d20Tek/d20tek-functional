using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace D20Tek.Functional.AspNetCore.WebApi;

public sealed class HandleResultFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context) =>
        GetObjectResult(context)
            .Bind(objRes => GetResultMonad(objRes))
            .Map(result => GetController(context)
                .Iter(controller => context.Result = ConvertToActionResult(result, controller).ToIActionResult()));

    private static Option<ObjectResult> GetObjectResult(ActionExecutedContext context) => 
        context.Result as ObjectResult ?? Option<ObjectResult>.None();

    public static Option<IResultMonad> GetResultMonad(ObjectResult objRes) =>
        objRes.Value is IResultMonad result ? Option<IResultMonad>.Some(result) : Option<IResultMonad>.None();

    public static Option<ControllerBase> GetController(ActionExecutedContext context) =>
        context.Controller is ControllerBase controller ? controller : Option<ControllerBase>.None();

    private static ActionResult<IResultMonad> ConvertToActionResult(IResultMonad result, ControllerBase controller) =>
        result.IsSuccess
            ? result.GetValue() is null ? controller.Ok() : controller.Ok(result.GetValue())
            : controller.Problem<IResultMonad>(result.GetErrors());
}
