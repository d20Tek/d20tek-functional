namespace D20Tek.Functional.AspNetCore.WebApi;

/// <summary>
/// An MVC action filter that automatically converts <see cref="IResultMonad"/> return values
/// from controller actions into appropriate <see cref="ActionResult"/> responses (OK or Problem Details).
/// Register globally or per-controller with <c>[ServiceFilter(typeof(HandleResultFilter))]</c>.
/// </summary>
public sealed class HandleResultFilter : IActionFilter
{
    /// <inheritdoc/>
    public void OnActionExecuting(ActionExecutingContext context) { }

    /// <inheritdoc/>
    public void OnActionExecuted(ActionExecutedContext context) =>
        GetObjectResult(context)
            .Bind(objRes => GetResultMonad(objRes))
            .Map(result => GetController(context)
                .Iter(controller => context.Result = ConvertToActionResult(result, controller).ToIActionResult()));

    private static Optional<ObjectResult> GetObjectResult(ActionExecutedContext context) => 
        context.Result as ObjectResult ?? Optional<ObjectResult>.None();

    /// <summary>
    /// Extracts an <see cref="IResultMonad"/> from the given <see cref="ObjectResult"/>, if present.
    /// </summary>
    public static Optional<IResultMonad> GetResultMonad(ObjectResult objRes) =>
        objRes.Value is IResultMonad result ? Optional<IResultMonad>.Some(result) : Optional<IResultMonad>.None();

    /// <summary>
    /// Extracts the <see cref="ControllerBase"/> from the action context, if available.
    /// </summary>
    public static Optional<ControllerBase> GetController(ActionExecutedContext context) =>
        context.Controller is ControllerBase controller ? controller : Optional<ControllerBase>.None();

    private static ActionResult<IResultMonad> ConvertToActionResult(IResultMonad result, ControllerBase controller) =>
        result.IsSuccess
            ? result.GetValue() is null ? controller.Ok() : controller.Ok(result.GetValue())
            : controller.Problem<IResultMonad>(result.GetErrors());
}
