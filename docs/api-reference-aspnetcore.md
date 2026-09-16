# ASP.NET Core API Reference

This reference documents the `D20Tek.Functional.AspNetCore` package. It builds on the core `Result<T>` and `Error` types to remove repetitive boilerplate when translating results into HTTP responses, supporting both Minimal APIs and MVC/Web API controllers. The descriptions are derived from the XML documentation comments in the source.

The package organizes its members into three areas:

- `D20Tek.Functional.AspNetCore` - error-to-status-code mapping.
- `D20Tek.Functional.AspNetCore.MinimalApi` - Minimal API (`IResult`) integration.
- `D20Tek.Functional.AspNetCore.WebApi` - MVC/Web API (`ActionResult<T>`) integration.

## Table of contents

- [Error type mapping](#error-type-mapping)
  - [IErrorTypeMapper](#ierrortypemapper)
  - [IErrorTypeConfigurator](#ierrortypeconfigurator)
  - [ErrorTypeMapper](#errortypemapper)
- [Minimal API integration](#minimal-api-integration)
  - [ResultExtensions](#minimalapi-resultextensions)
  - [ResultAsyncExtensions](#minimalapi-resultasyncextensions)
  - [TypedResultExtensions](#typedresultextensions)
  - [HandleResultFilter](#minimalapi-handleresultfilter)
  - [HandleTypedResultFilter&lt;T&gt;](#handletypedresultfiltert)
- [Web API integration](#web-api-integration)
  - [ResultExtensions](#webapi-resultextensions)
  - [ResultAsyncExtensions](#webapi-resultasyncextensions)
  - [ActionResultExtensions](#actionresultextensions)
  - [HandleResultFilter](#webapi-handleresultfilter)

---

## Error type mapping

### IErrorTypeMapper

Maps `ErrorType` integer values to `System.Net.HttpStatusCode` values. Used by the ASP.NET Core integration to convert domain errors into appropriate HTTP responses. Access the singleton instance via `ErrorTypeMapper.Instance`.

| Member | Description |
| --- | --- |
| `IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null)` | Reconfigures the mapper using the provided configurator action. Passing `null` resets to default mappings. |
| `bool Contains(int errorType)` | Returns whether a mapping exists for the specified error type. |
| `HttpStatusCode Convert(int errorType)` | Converts an error type to its mapped HTTP status code. Returns `HttpStatusCode.InternalServerError` if no mapping exists. |
| `IErrorTypeMapper For(int errorType, HttpStatusCode statusCode)` | Adds or overrides a mapping from an error type to an HTTP status code. |
| `IErrorTypeMapper Remove(int errorType)` | Removes the mapping for the specified error type. |

### IErrorTypeConfigurator

Fluent interface for configuring the mapping between `ErrorType` integer values and `HttpStatusCode` responses. Used within `IErrorTypeMapper.Configure`.

| Member | Description |
| --- | --- |
| `IErrorTypeConfigurator Clear()` | Removes all existing error-type-to-status-code mappings. |
| `IErrorTypeConfigurator For(int errorType, HttpStatusCode statusCode)` | Maps (or overrides) an error type to a specific HTTP status code. |
| `IErrorTypeConfigurator Remove(int errorType)` | Removes the mapping for the specified error type. |

### ErrorTypeMapper

Singleton implementation of `IErrorTypeMapper` that maintains a global mapping between `ErrorType` integer values and HTTP status codes. Access the instance via `Instance` and reconfigure with `Configure`.

| Member | Description |
| --- | --- |
| `static IErrorTypeMapper Instance` | Gets the singleton `IErrorTypeMapper` instance with default mappings applied. |
| `IErrorTypeMapper For(int errorType, HttpStatusCode statusCode)` | Adds or overrides a mapping (see `IErrorTypeMapper`). |
| `IErrorTypeMapper Remove(int errorType)` | Removes a mapping. |
| `bool Contains(int errorType)` | Returns whether a mapping exists. |
| `HttpStatusCode Convert(int errorType)` | Converts an error type to its mapped HTTP status code. |
| `IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null)` | Reconfigures the mapper. |

---

## Minimal API integration

### <a id="minimalapi-resultextensions"></a>ResultExtensions (MinimalApi)

Extension methods for converting `Result<T>` into Minimal API `IResult` responses. Supports OK, Created, and CreatedAtRoute response patterns.

| Member | Description |
| --- | --- |
| `IResult ToApiResult<TValue>(this Result<TValue> result)` | Converts to `IResult`: 200 OK on success, or Problem Details on failure. |
| `IResult ToApiResult<TValue, TResponse>(this Result<TValue> result, Func<TValue, TResponse> responseMap)` | Converts to `IResult`, mapping the success value to a response DTO. |
| `IResult ToApiResult<TValue, TResponse>(this Result<TValue> result, TResponse response)` | Converts to `IResult`, returning a fixed response object on success. |
| `IResult ToCreatedApiResult<TValue>(this Result<TValue> result, string? routeName = null, object? routeValues = null)` | Converts to a 201 CreatedAtRoute response on success, or Problem Details on failure. |
| `IResult ToCreatedApiResult<TValue, TResponse>(this Result<TValue> result, Func<TValue, TResponse> responseMap, ...)` | Converts to a 201 CreatedAtRoute response, mapping the success value to a response DTO. |
| `IResult ToCreatedApiResult<TValue, TResponse>(this Result<TValue> result, TResponse response, ...)` | Converts to a 201 CreatedAtRoute response with a fixed response object. |

### <a id="minimalapi-resultasyncextensions"></a>ResultAsyncExtensions (MinimalApi)

Async counterparts that operate on `Task<Result<TValue>>`, so results produced by async handlers can be converted without an intermediate `await`.

| Member | Description |
| --- | --- |
| `Task<IResult> ToApiResultAsync<TValue>(this Task<Result<TValue>> result)` | Awaits the result and converts it to an OK/Problem `IResult`. |
| `Task<IResult> ToApiResultAsync<TValue, TResponse>(...)` | Awaits and converts, mapping the success value to a response DTO or fixed response. |
| `Task<IResult> ToCreatedApiResultAsync<TValue>(...)` | Awaits and converts to a 201 CreatedAtRoute response. |
| `Task<IResult> ToCreatedApiResultAsync<TValue, TResponse>(...)` | Awaits and converts to a 201 CreatedAtRoute response with a mapped or fixed response. |

### TypedResultExtensions

Extension methods on `IResultExtensions` for converting `Error` arrays into RFC 7807 Problem Details responses in Minimal APIs.

| Member | Description |
| --- | --- |
| `IResult Problem(this IResultExtensions _, IEnumerable<Error> errors)` | Converts a collection of errors into a Problem Details response. Validation-only errors produce a 400 ValidationProblem; other errors produce a standard Problem response. |
| `IResult Problem(this IResultExtensions _, Error error)` | Converts a single error into a Problem Details response. |
| `IResult Problem(this IResultExtensions _, int statusCode, string errorCode, string message)` | Creates a Problem Details response with an explicit status code, error code, and message. |

Invoke these via `Results.Extensions.Problem(...)`.

### <a id="minimalapi-handleresultfilter"></a>HandleResultFilter

An endpoint filter for Minimal APIs that automatically converts `IResultMonad` return values into appropriate HTTP responses (200 OK or Problem Details). Register with `.AddEndpointFilter<HandleResultFilter>()` on route handlers.

| Member | Description |
| --- | --- |
| `ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)` | Invokes the next filter and, if the result is an `IResultMonad`, converts it to an `IResult`. |

### HandleTypedResultFilter&lt;T&gt;

A typed endpoint filter for Minimal APIs that converts `Result<T>` return values into appropriate HTTP responses. Use when the endpoint's return type is known at compile time. Register with `.AddEndpointFilter<HandleTypedResultFilter<T>>()`. `T` must be a reference type.

| Member | Description |
| --- | --- |
| `ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)` | Invokes the next filter and, if the result is a `Result<T>`, converts it to a 200 OK or Problem Details response. |

---

## Web API integration

### <a id="webapi-resultextensions"></a>ResultExtensions (WebApi)

Extension methods for converting `Result<T>` into `ActionResult<T>` responses in Web API controllers. Supports OK, Created, and CreatedAtAction patterns.

| Member | Description |
| --- | --- |
| `ActionResult<TResponse> ToActionResult<TValue, TResponse>(this Result<TValue> result, Func<TValue, TResponse> responseMap, ControllerBase controller)` | Converts to an `ActionResult<T>`, mapping the success value to a response DTO. |
| `ActionResult<TResponse> ToActionResult<TValue, TResponse>(this Result<TValue> result, TResponse response, ControllerBase controller)` | Converts to an `ActionResult<T>`, returning a fixed response on success. |
| `ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(this Result<TValue> result, Func<TValue, TResponse> responseMap, ControllerBase controller, string? routeName = null, object? routeValues = null)` | Converts to a 201 CreatedAtAction response, mapping the success value. |
| `ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(this Result<TValue> result, TResponse response, ControllerBase controller, ...)` | Converts to a 201 CreatedAtAction response with a fixed response object. |

### <a id="webapi-resultasyncextensions"></a>ResultAsyncExtensions (WebApi)

Async counterparts that operate on `Task<Result<TValue>>`.

| Member | Description |
| --- | --- |
| `Task<ActionResult<TResponse>> ToActionResultAsync<TValue, TResponse>(...)` | Awaits the result and converts it to an `ActionResult<T>`, with a mapped or fixed response. |
| `Task<ActionResult<TResponse>> ToCreatedActionResultAsync<TValue, TResponse>(...)` | Awaits the result and converts it to a 201 CreatedAtAction response, with a mapped or fixed response. |

### ActionResultExtensions

Extension methods for `ControllerBase` that convert `Error` arrays into RFC 7807 Problem Details `ActionResult<T>` responses for Web API controllers.

| Member | Description |
| --- | --- |
| `ActionResult<TResult> Problem<TResult>(this ControllerBase controller, IEnumerable<Error> errors)` | Converts a collection of errors into a Problem Details `ActionResult<T>`. Validation-only errors produce a 400 ValidationProblem; other errors produce a standard Problem response. |
| `ActionResult<TResult> Problem<TResult>(this ControllerBase controller, Error error)` | Converts a single error into a Problem Details `ActionResult<T>`. |
| `ActionResult<TResult> Problem<TResult>(this ControllerBase controller, int statusCode, string errorCode, string message)` | Creates a Problem Details response with an explicit status code, error code, and message. |
| `IActionResult ToIActionResult(this IConvertToActionResult actionResult)` | Converts an `IConvertToActionResult` to an `IActionResult`. |
| `Task<IActionResult> ToIActionResult(this Task<ActionResult<T>> ...)` | Asynchronously converts a `Task<ActionResult<T>>` to an `IActionResult`. |

### <a id="webapi-handleresultfilter"></a>HandleResultFilter

An MVC action filter that automatically converts `IResultMonad` return values from controller actions into appropriate `ActionResult` responses (OK or Problem Details). Register globally or per-controller with `[ServiceFilter(typeof(HandleResultFilter))]`.

| Member | Description |
| --- | --- |
| `void OnActionExecuting(ActionExecutingContext context)` | No-op; part of the `IActionFilter` contract. |
| `void OnActionExecuted(ActionExecutedContext context)` | Converts an `IResultMonad` returned in an `ObjectResult` into an `ActionResult` response. |
| `static Optional<IResultMonad> GetResultMonad(ObjectResult objRes)` | Extracts an `IResultMonad` from the given `ObjectResult`, if present. |
