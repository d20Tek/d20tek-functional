# Core API Reference

This reference documents the primary types in the `D20Tek.Functional` namespace. The descriptions are derived from the XML documentation comments in the source.

## Table of contents

- [Optional&lt;T&gt;](#optionalt)
- [Result&lt;T&gt;](#resultt)
- [Result (factory)](#result-factory)
- [Unit](#unit)
- [Error](#error)
- [ErrorType](#errortype)
- [ValidationErrors](#validationerrors)
- [Choice&lt;T1, T2&gt;](#choicet1-t2)
- [Identity&lt;T&gt;](#identityt)
- [State (IState and StateExtensions)](#state-istate-and-stateextensions)
- [TryExcept](#tryexcept)
- [FunctionalExtensions](#functionalextensions)
- [BooleanExtensions](#booleanextensions)
- [IEnumerableExtensions](#ienumerableextensions)

---

## Optional&lt;T&gt;

A discriminated union representing an optional value: either `Some<T>` containing a value or `None<T>` representing absence. This is the Option monad inspired by F#'s Option type, providing a type-safe alternative to null references.

`T` must be `notnull`.

### Factory members

| Member | Description |
| --- | --- |
| `static Optional<T> Some(T value)` | Creates an `Optional<T>` containing the specified value. |
| `static Optional<T> None()` | Creates an empty `Optional<T>` representing the absence of a value. |
| `implicit operator Optional<T>(T instance)` | Implicitly converts a value into an `Optional<T>`. Null values become None; non-null values become Some. |

### Properties

| Member | Description |
| --- | --- |
| `bool IsSome` | Gets whether this optional contains a value. |
| `bool IsNone` | Gets whether this optional is empty (no value). |

### Methods

| Member | Description |
| --- | --- |
| `TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)` | Pattern-matches on the optional, invoking `onSome` if a value is present or `onNone` if empty. |
| `Optional<TResult> Bind<TResult>(Func<T, Optional<TResult>> bind)` | Monadic bind: if a value is present, passes it to `bind` which returns a new optional; otherwise returns None. |
| `bool Contains(T value)` | Returns `true` if this optional contains a value equal to `value`. |
| `int Count()` | Returns 1 if this optional contains a value, 0 otherwise. |
| `T DefaultValue(T defaultArg)` | Returns the contained value if present, otherwise `defaultArg`. |
| `T DefaultWith(Func<T> func)` | Returns the contained value if present, otherwise invokes `func` to produce a fallback. |
| `bool Exists(Func<T, bool> predicate)` | Returns `true` if the optional contains a value satisfying `predicate`. |
| `Optional<T> Filter(Func<T, bool> predicate)` | Returns Some if the value satisfies `predicate`, otherwise None. |
| `TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func)` | Left-fold over the contained value starting from `initial`. Returns `initial` if empty. |
| `TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func)` | Right-fold over the contained value starting from `initial`. Returns `initial` if empty. |
| `bool ForAll(Func<T, bool> predicate)` | Returns `true` if the optional is empty or the value satisfies `predicate`. |
| `T Get()` | Extracts the contained value. Throws `ArgumentNullException` if empty. Prefer `Match` or `DefaultValue`. |
| `Optional<T> Iter(Action<T> action)` | Executes a side-effect action on the contained value (if present) and returns this optional unchanged. |
| `Optional<TResult> Map<TResult>(Func<T, TResult> mapper)` | Transforms the contained value using `mapper`. If empty, returns None. |
| `Optional<T> OrElse(Optional<T> ifNone)` | Returns this optional if it contains a value, otherwise `ifNone`. |
| `Optional<T> OrElseWith(Func<Optional<T>> ifNone)` | Returns this optional if it contains a value, otherwise invokes `ifNone`. |
| `T[] ToArray()` | Returns a single-element array containing the value, or an empty array if empty. |
| `ImmutableList<T> ToList()` | Returns an immutable list containing the value, or an empty list if empty. |
| `T? ToNullable()` | Returns the contained value as a nullable reference, or `null` if empty. |
| `T? ToObj()` | Alias for `ToNullable`. |
| `override string ToString()` | Returns the string representation of the optional. |

---

## Result&lt;T&gt;

A discriminated union representing either a successful value of type `T` or an array of `Error` instances. This is the core Result monad that enables railway-oriented programming: chain operations with `Bind` and `Map`, and resolve the final outcome with `Match`.

`T` must be `notnull`. Implements `IResultMonad`.

### Factory members

| Member | Description |
| --- | --- |
| `static Result<T> Success(T value)` | Creates a successful result containing the specified value. |
| `static Result<T> Failure(Error[] errors)` | Creates a failed result containing the specified errors. |
| `static Result<T> Failure(Error error)` | Creates a failed result containing a single error. |
| `static Result<T> Failure(Exception ex)` | Creates a failed result from an exception, converting it to an `Error`. |
| `implicit operator Result<T>(T instance)` | Implicitly converts a value into a successful `Result<T>`. |

### Properties

| Member | Description |
| --- | --- |
| `bool IsSuccess` | Gets whether this result represents a successful outcome. |
| `bool IsFailure` | Gets whether this result represents a failed outcome. |

### Methods

| Member | Description |
| --- | --- |
| `TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error[], TResult> onFailure)` | Pattern-matches on the result, invoking `onSuccess` if successful or `onFailure` if failed. |
| `Result<TResult> Bind<TResult>(Func<T, Result<TResult>> bind)` | Monadic bind: if successful, passes the value to `bind`; if failed, propagates the errors. |
| `int Count()` | Returns 1 if successful, 0 otherwise. |
| `T DefaultValue(T defaultArg)` | Returns the success value if present, otherwise `defaultArg`. |
| `T DefaultWith(Func<T> func)` | Returns the success value if present, otherwise invokes `func`. |
| `bool Exists(Func<T, bool> predicate)` | Returns `true` if successful and `predicate` is satisfied. |
| `Result<T> Filter(Func<T, bool> predicate)` | Returns a successful result if the value satisfies `predicate`; otherwise a failure with a "not found" filter error. |
| `TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func)` | Left-fold over the success value starting from `initial`. Returns `initial` on failure. |
| `TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func)` | Right-fold over the success value starting from `initial`. Returns `initial` on failure. |
| `bool ForAll(Func<T, bool> predicate)` | Returns `true` if the result is a failure or the success value satisfies `predicate`. |
| `T GetValue()` | Extracts the success value. Throws `ArgumentNullException` on failure. Prefer `Match` or `DefaultValue`. |
| `Error[] GetErrors()` | Returns the error array on failure, or an empty array if successful. |
| `Result<T> Iter(Action<T> action)` | Executes a side-effect action on the success value (if present) and returns this result unchanged. |
| `Result<TResult> Map<TResult>(Func<T, TResult> mapper)` | Transforms the success value using `mapper`. If failed, propagates errors. |
| `Result<TResult> MapErrors<TResult>()` | Transfers the error array to a new `Result<TResult>`. Throws `InvalidOperationException` if called on a successful result. |
| `T[] ToArray()` | Returns a single-element array with the success value, or an empty array on failure. |
| `ImmutableList<T> ToList()` | Returns an immutable list with the success value, or an empty list on failure. |
| `Optional<T> ToOptional()` | Converts to an `Optional<T>`: Some if successful, None if failure. Error information is discarded. |
| `override string ToString()` | Returns the string representation of the result. |

---

## Result (factory)

Provides non-generic factory helpers for creating `Result<T>` instances, with convenience methods for side-effect-only operations that produce a `Unit` value.

| Member | Description |
| --- | --- |
| `static Result<Unit> Success()` | Creates a successful `Result<Unit>`, representing a side-effect operation that completed without a meaningful return value. |
| `static Result<T> Success<T>(T value)` | Creates a successful `Result<T>` containing the specified value; the value type is inferred. |
| `static Result<Unit> Failure(Error[] errors)` | Creates a failed `Result<Unit>` containing the specified errors. |
| `static Result<Unit> Failure(Error error)` | Creates a failed `Result<Unit>` containing a single error. |
| `static Result<Unit> Failure(Exception ex)` | Creates a failed `Result<Unit>` from an exception. |

---

## Unit

Represents the absence of a meaningful value - the functional equivalent of `void`. Because C# generics cannot use `void` as a type argument, `Unit` allows monadic types such as `Result<T>` to represent side-effect-only operations (for example, `Result<Unit>`) without inventing a dummy value.

`Unit` is a `readonly struct` implementing `IEquatable<Unit>`.

| Member | Description |
| --- | --- |
| `static Unit Value` | Gets the singleton `Unit` value. |
| `implicit operator Unit(ValueTuple _)` | Implicitly converts an empty `ValueTuple` into a `Unit`. |
| `bool Equals(Unit other)` | Determines whether this instance is equal to another `Unit`. All `Unit` values are equal. |
| `override bool Equals(object? obj)` | Determines whether the specified object is a `Unit`. |
| `override int GetHashCode()` | Returns a constant hash code, since all `Unit` values are equal. |
| `operator ==` / `operator !=` | Equality operators. `==` always returns `true`, `!=` always returns `false`. |
| `override string ToString()` | Returns `()`, following the F# convention. |

---

## Error

Represents a structured error with a type classification, code identifier, and human-readable message. Use this value type to convey domain errors through `Result<T>` without throwing exceptions, enabling railway-oriented programming patterns.

`Error` is a `readonly struct`.

### Properties

| Member | Description |
| --- | --- |
| `int Type` | The numeric error type classification (see `ErrorType`). Used by error-to-HTTP-status-code mappers. |
| `string Code` | The machine-readable error code (e.g., "User.NotFound"). |
| `string Message` | The human-readable error message. |

### Factory methods

| Member | Description |
| --- | --- |
| `static Error Unexpected(string code, string message)` | Unexpected/unhandled failure (HTTP 500 equivalent). |
| `static Error Failure(string code, string message)` | General operation failure (HTTP 400 equivalent). |
| `static Error Validation(string code, string message)` | Validation failure (HTTP 400 equivalent). |
| `static Error NotFound(string code, string message)` | Requested resource not found (HTTP 404 equivalent). |
| `static Error Conflict(string code, string message)` | Resource conflict (HTTP 409 equivalent). |
| `static Error Unauthorized(string code, string message)` | Caller not authenticated (HTTP 401 equivalent). |
| `static Error Forbidden(string code, string message)` | Caller lacks permission (HTTP 403 equivalent). |
| `static Error Invalid(string code, string message)` | Semantically invalid request (HTTP 422 equivalent). |
| `static Error Exception(Exception ex)` | Creates an `Error` from a caught exception, classified as `ErrorType.Unexpected`. |
| `static Error Create(string code, string message, int errorType)` | Creates an error with a custom numeric error type. |
| `override string ToString()` | Returns the string representation of the error. |

---

## ErrorType

Defines well-known numeric error type constants used by `Error`. These values map to standard HTTP status code categories when used with the ASP.NET Core integrations. You can extend this set with custom integer values via `Error.Create`.

| Constant | Value | Maps to |
| --- | --- | --- |
| `Unexpected` | 0 | HTTP 500 |
| `Failure` | 1 | HTTP 400 |
| `Validation` | 2 | HTTP 400 |
| `NotFound` | 3 | HTTP 404 |
| `Conflict` | 4 | HTTP 409 |
| `Unauthorized` | 5 | HTTP 401 |
| `Forbidden` | 6 | HTTP 403 |
| `Invalid` | 7 | HTTP 422 |

---

## ValidationErrors

A fluent builder for accumulating validation errors before producing a `Result<T>`. Chain multiple `AddIfError` calls to collect all validation failures, then call `Map` or `Bind` to either produce a success value or a failure containing all errors.

| Member | Description |
| --- | --- |
| `bool HasErrors` | Gets whether any validation errors have been accumulated. |
| `static ValidationErrors Create()` | Creates a new empty instance to begin accumulating checks. |
| `ValidationErrors AddIfError(Func<bool> check, Error error)` | If `check` returns `true`, adds `error`. Returns this instance for chaining. |
| `ValidationErrors AddIfError(Func<bool> check, string code, string message)` | If `check` returns `true`, adds a validation error with the given code and message. |
| `Result<T> Map<T>(Func<T> onSuccess)` | If no errors, invokes `onSuccess` and wraps the result in a success; otherwise a failure containing all errors. |
| `Result<T> Bind<T>(Func<Result<T>> onSuccess)` | If no errors, invokes `onSuccess` (which returns a Result); otherwise a failure containing all errors. |
| `Error[] ToArray()` | Returns the accumulated errors as an array. |
| `Result<T> ToFailure<T>()` | Converts the accumulated errors into a failure `Result<T>`. |

---

## Choice&lt;T1, T2&gt;

A discriminated union holding exactly one value of either `T1` or `T2`. Use Choice when a value can legitimately be one of two types and you want exhaustive pattern matching via `Match` rather than type-checking or exceptions.

`T1` and `T2` must be `notnull`. `T1` is the primary/success path for `Bind`/`Map`.

| Member | Description |
| --- | --- |
| `Choice(T1 value)` / `Choice(T2 value)` | Creates a Choice holding a value of the first or second type. |
| `bool IsChoice1` / `bool IsChoice2` | Gets whether the stored value is of the first or second type. |
| `TResult Match<TResult>(Func<T1, TResult> func1, Func<T2, TResult> func2)` | Exhaustively pattern-matches, invoking the appropriate handler based on the stored type. |
| `Choice<T1, T2> Iter(Action<T1> action1, Action<T2> action2)` | Executes a side-effect action based on which type is stored, then returns this Choice. |
| `Choice<TResult, T2> Bind<TResult>(Func<T1, Choice<TResult, T2>> bindFunc)` | Monadic bind on the first type; propagates the second type unchanged. |
| `T1 GetChoice1()` | Extracts the value as the first type. Throws `InvalidCastException` if it is the second type. |
| `T2 GetChoice2()` | Extracts the value as the second type. Throws `InvalidCastException` if it is the first type. |

---

## Identity&lt;T&gt;

The Identity monad: a simple wrapper around a single non-null value of type `T`. Use Identity to lift plain values into a monadic pipeline, enabling uniform composition with `Bind`, `Map`, and `Iter`.

`T` must be `notnull`.

| Member | Description |
| --- | --- |
| `Identity(T value)` | Constructs an Identity wrapping the value. |
| `static Identity<T> Create(T value)` | Factory method to create a new `Identity<T>`. |
| `implicit operator Identity<T>(T instance)` | Implicitly wraps a value into an `Identity<T>`. |
| `implicit operator T(Identity<T> instance)` | Implicitly extracts the value from an `Identity<T>`. |
| `Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> bind)` | Monadic bind: passes the value to `bind` which returns a new Identity. |
| `bool Contains(T value)` | Returns `true` if the contained value equals `value`. |
| `int Count()` | Always returns 1. |
| `bool Exists(Func<T, bool> predicate)` | Returns `true` if the contained value satisfies `predicate`. |
| `T Get()` | Extracts the contained value. |
| `Identity<T> Iter(Action<T> action)` | Executes a side-effect action on the value and returns this Identity unchanged. |
| `Identity<TResult> Map<TResult>(Func<T, TResult> mapper)` | Transforms the value using `mapper` and wraps it in a new Identity. |
| `override string ToString()` | Returns the string representation of the Identity. |

---

## State (IState and StateExtensions)

Support for the State monad over immutable state transitions.

### IState

Marker interface for state objects that participate in the State monad pipeline. Implement this interface on your immutable state records/classes to enable the `StateExtensions` methods.

### StateExtensions

Extension methods providing monadic operations (Bind, Map, Iter) for types implementing `IState`. These enable functional pipelines over immutable state transitions.

| Member | Description |
| --- | --- |
| `TOut Bind<TIn, TOut>(this TIn state, Func<TIn, TOut> bind)` | Monadic bind: transforms the current state into a new state type. Both `TIn` and `TOut` must implement `IState`. |
| `T Iter<T>(this T state, Action<T> action)` | Executes a side-effect action on the state and returns it unchanged. `T` must implement `IState`. |
| `TOut Map<TIn, TOut>(this TIn state, Func<TIn, TOut> mapper)` | Maps the current state to a non-state value. Use to extract a final result from a state pipeline. |

---

## TryExcept

Provides functional try/catch/finally wrappers that return values or `Result<T>` instead of throwing exceptions. Enables exception handling as expressions rather than statements.

| Member | Description |
| --- | --- |
| `T Run<T>(Func<T> operation, Func<Exception, T> onException, Action? onFinally = null)` | Executes `operation` and returns its result; on exception, invokes `onException` for a fallback value. |
| `void Run(Action operation, Action<Exception> onException, Action? onFinally = null)` | Executes `operation`; on exception, invokes `onException`. |
| `Result<TResult> Bind<T, TResult>(Func<T> operation, Func<T, Result<TResult>> bind)` | Executes `operation`, passes its result to `bind`, and returns the Result. On exception, returns a failure Result. |

Additional overloads that wrap operations into a success Result via a mapper are also provided.

---

## FunctionalExtensions

General-purpose functional programming extension methods that work on any type. Includes pipeline operators (Pipe), parallel composition (Fork), alternative selection (Alt), and iterative looping (IterateUntil).

| Member | Description |
| --- | --- |
| `TOut? Alt<TIn, TOut>(this TIn instance, params Func<TIn, TOut>[] args)` | Tries each function in order, returning the first non-null result. Returns `null` if all return null. |
| `TOut Fork<TIn, T1, T2, TOut>(this TIn instance, Func<TIn, T1> f1, Func<TIn, T2> f2, Func<T1, T2, TOut> fOut)` | Applies two independent functions to the same input, then combines their results. |
| `void Fork<TIn, T1, T2>(this TIn instance, Func<TIn, T1> f1, Func<TIn, T2> f2, Action<T1, T2> fOut)` | Applies two independent functions to the same input, then combines their results via an action. |
| `TResult Pipe<T, TResult>(this T instance, Func<T, TResult> func)` | Passes the instance through a transformation function. Equivalent to F#'s pipe-forward operator (`\|>`). |
| `T Pipe<T>(this T instance, Action<T> action)` | Executes a side-effect action on the instance and returns it unchanged. |
| `T IterateUntil<T>(this T instance, Func<T, T> updateFunction, Func<T, bool> endCondition)` | Repeatedly applies `updateFunction` until `endCondition` is met. |
| `Result<T> IterateUntil<T>(this T instance, Func<T, Result<T>> updateFunction, Func<T, bool> endCondition)` | Repeatedly applies a failable `updateFunction` until `endCondition` is met or a failure occurs; returns the final Result. |

---

## BooleanExtensions

Extension methods for `bool` that enable functional branching without if/else statements.

| Member | Description |
| --- | --- |
| `TOut IfTrueOrElse<TOut>(this bool condition, Func<TOut> thenFunc, Func<TOut> elseFunc)` | Evaluates one of two functions based on the boolean value and returns the result. |
| `void IfTrueOrElse(this bool condition, Action thenAction, Action? elseAction = null)` | Executes one of two actions based on the boolean value. `elseAction` is optional and defaults to no-op. |

---

## IEnumerableExtensions

Extension methods for `IEnumerable<T>` providing functional utilities.

| Member | Description |
| --- | --- |
| `string AsString<T>(this IEnumerable<T> list, string defaultMessage = "")` | Joins all elements into a comma-separated string, or returns `defaultMessage` if the collection is empty. |
| `void ForEach<TIn>(this IEnumerable<TIn> enumerable, Action<TIn> action)` | Executes `action` for each element. A functional alternative to a foreach loop. |
