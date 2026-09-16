# Async API Reference

This reference documents the `D20Tek.Functional.Async` namespace. These types and extension methods mirror the synchronous core operators for `Task`-based asynchronous code, enabling clean composition of async pipelines. The descriptions are derived from the XML documentation comments in the source.

Most async extension methods provide two overloads:

- One that extends the synchronous type (for example, `Result<T>`) and accepts an async handler.
- One that extends the `Task`-wrapped type (for example, `Task<Result<T>>`) so calls can be chained fluently without intermediate `await` statements.

## Table of contents

- [ResultAsyncExtensions](#resultasyncextensions)
- [OptionalAsyncExtensions](#optionalasyncextensions)
- [IdentityAsyncExtensions](#identityasyncextensions)
- [StateAsyncExtensions](#stateasyncextensions)
- [ValidationErrorsExtensions](#validationerrorsextensions)
- [ChoiceAsync and multi-arity Choice types](#choiceasync-and-multi-arity-choice-types)
- [TryExceptAsync](#tryexceptasync)
- [TaskExtensions](#taskextensions)
- [BoolAsyncExtensions](#boolasyncextensions)
- [FunctionalAsyncExtensions](#functionalasyncextensions)

---

## ResultAsyncExtensions

Async extension methods for `Result<T>`, mirroring the synchronous `Result<T>` operators.

| Member | Description |
| --- | --- |
| `Task<TResult> MatchAsync<TIn, TResult>(...)` | Asynchronously pattern-matches on the result, invoking the success or failure handler. |
| `Task<Result<TResult>> BindAsync<TIn, TResult>(...)` | Async monadic bind: if successful, passes the value to the async binder; if failed, propagates the errors. |
| `Task<T> DefaultWithAsync<T>(...)` | Returns the success value if present, otherwise awaits `func` to produce a fallback. |
| `Task<bool> ExistsAsync<T>(...)` | Returns `true` if successful and the async predicate is satisfied. |
| `Task<Result<T>> FilterAsync<T>(...)` | Returns a successful result if the value satisfies the async predicate; otherwise a filter failure. |
| `Task<TResult> FoldAsync<T, TResult>(...)` | Async left-fold over the success value starting from an initial accumulator. |
| `Task<TResult> FoldBackAsync<T, TResult>(...)` | Async right-fold over the success value starting from an initial accumulator. |
| `Task<bool> ForAllAsync<T>(...)` | Returns `true` if the result is a failure or the success value satisfies the async predicate. |
| `Task<Result<T>> IterAsync<T>(...)` | Executes an async side-effect on the success value (if present) and returns the result unchanged. |
| `Task<Result<TResult>> MapAsync<TIn, TResult>(...)` | Transforms the success value using an async mapper. If failed, propagates errors. |
| `Task<Result<TResult>> MapErrorsAsync<TIn, TResult>(this Task<Result<TIn>> result)` | Transfers the error array to a new `Result<TResult>` of a different type. |

Each transforming method provides overloads on both `Result<T>` and `Task<Result<T>>`.

---

## OptionalAsyncExtensions

Async extension methods for `Optional<T>`, mirroring the synchronous `Optional<T>` operators.

| Member | Description |
| --- | --- |
| `Task<TResult> MatchAsync<T, TResult>(...)` | Asynchronously pattern-matches on the optional, invoking the some or none handler. |
| `Task<Optional<TResult>> BindAsync<T, TResult>(...)` | Async monadic bind: if a value is present, passes it to the async binder; otherwise returns None. |
| `Task<T> DefaultWithAsync<T>(...)` | Returns the contained value if present, otherwise awaits `func` to produce a fallback. |
| `Task<bool> ExistsAsync<T>(...)` | Returns `true` if the optional contains a value satisfying the async predicate. |
| `Task<Optional<T>> FilterAsync<T>(...)` | Returns Some if the value satisfies the async predicate, otherwise None. |
| `Task<TResult> FoldAsync<T, TResult>(...)` | Async left-fold over the contained value. |
| `Task<TResult> FoldBackAsync<T, TResult>(...)` | Async right-fold over the contained value. |
| `Task<bool> ForAllAsync<T>(...)` | Returns `true` if the optional is empty or the value satisfies the async predicate. |
| `Task<Optional<T>> IterAsync<T>(...)` | Executes an async side-effect on the contained value (if present) and returns the optional unchanged. |
| `Task<Optional<TResult>> MapAsync<T, TResult>(...)` | Transforms the contained value using an async mapper. If empty, returns None. |
| `Task<Optional<T>> OrElseWithAsync<T>(...)` | Returns this optional if it contains a value, otherwise awaits `ifNone` for an alternative. |

Each transforming method provides overloads on both `Optional<T>` and `Task<Optional<T>>`.

---

## IdentityAsyncExtensions

Async extension methods for `Identity<T>`.

| Member | Description |
| --- | --- |
| `Task<Identity<TResult>> BindAsync<T, TResult>(...)` | Async monadic bind: passes the value to the async binder which returns a new Identity. |
| `Task<bool> ExistsAsync<T>(...)` | Returns `true` if the contained value satisfies the async predicate. |
| `Task<Identity<T>> IterAsync<T>(...)` | Executes an async side-effect on the value and returns the Identity unchanged. |
| `Task<Identity<TResult>> MapAsync<T, TResult>(...)` | Transforms the value using an async mapper and wraps it in a new Identity. |

Each method provides overloads on both `Identity<T>` and `Task<Identity<T>>`.

---

## StateAsyncExtensions

Async extension methods providing monadic operations for types implementing `IState`.

| Member | Description |
| --- | --- |
| `Task<TOut> BindAsync<TIn, TOut>(...)` | Async monadic bind: transforms the current state into a new state type. Both types must implement `IState`. |
| `Task<T> IterAsync<T>(...)` | Executes an async side-effect on the state and returns it unchanged. |
| `Task<TOut> MapAsync<TIn, TOut>(...)` | Maps the current state to a non-state value using an async mapper. |

Each method provides overloads on both the state type and its `Task`-wrapped form.

---

## ValidationErrorsExtensions

Async extension methods for the `ValidationErrors` builder.

| Member | Description |
| --- | --- |
| `Task<ValidationErrors> AddIfErrorAsync(...)` | Awaits an async check; if it fails, adds the corresponding error. Overloads accept an `Error` or a code/message pair, and extend both `ValidationErrors` and `Task<ValidationErrors>`. |
| `Task<Result<T>> MapAsync<T>(...)` | If no errors, awaits `onSuccess` and wraps the value in a success; otherwise a failure containing all errors. |
| `Task<Result<T>> BindAsync<T>(...)` | If no errors, awaits `onSuccess` (which returns a Result); otherwise a failure containing all errors. |

---

## ChoiceAsync and multi-arity Choice types

Async-aware discriminated unions that hold exactly one value out of several possible types. They provide `MatchAsync`, `BindAsync`, `MapAsync`, and `IterAsync` for asynchronous pattern matching and transformation.

| Type | Description |
| --- | --- |
| `ChoiceAsync<T1, T2>` | Async discriminated union of two types. `T1` is the primary path for Bind/Map. |
| `Choice3Async<T1, T2, T3>` | Async discriminated union of three types. |
| `Choice4Async<T1, T2, T3, T4>` | Async discriminated union of four types. |
| `Choice5Async<T1, T2, T3, T4, T5>` | Async discriminated union of five types. |

Common members (illustrated for `ChoiceAsync<T1, T2>`):

| Member | Description |
| --- | --- |
| `ChoiceAsync(T1 value)` ... | Constructors, one per possible stored type. |
| `bool IsChoice1` ... | Gets whether the stored value is of the corresponding type. |
| `Task<TResult> MatchAsync<TResult>(...)` | Asynchronously pattern-matches, invoking the appropriate async handler. |
| `Task<ChoiceAsync<TResult, T2>> BindAsync<TResult>(...)` | Async monadic bind on the first type; propagates the other types unchanged. |
| `MapAsync` | Asynchronously transforms the stored value. |
| `IterAsync` | Executes async side-effect actions based on which type is stored. |
| `GetChoice1()` ... | Extracts the value as a specific type. Throws if the stored type differs. |

---

## TryExceptAsync

Provides async try/catch/finally wrappers that return values or `Result<T>` instead of throwing exceptions, enabling functional error handling in async code.

| Member | Description |
| --- | --- |
| `Task<T> RunAsync<T>(Func<Task<T>> operation, Func<Exception, T> onException, Action? onFinally = null)` | Executes an async operation and returns its result; on exception, invokes `onException` for a fallback value. |
| `RunAsync` (Task overload) | Executes an async operation with no return value; on exception, invokes the async/exception handler. |
| `BindAsync` / `MapAsync` variants | Execute an async operation and transform the result into a `Result<T>`, converting any exception into a failure Result. |

---

## TaskExtensions

Extension methods for `Task<T>` enabling fluent async pipelines.

| Member | Description |
| --- | --- |
| `Task<TOut> ThenAsync<TIn, TOut>(this Task<TIn> task, Func<TIn, Task<TOut>> func)` | Chains an async function onto a task, awaiting the task and passing the result to `func`. |

---

## BoolAsyncExtensions

Async extension methods for `bool` enabling functional branching with asynchronous handlers.

| Member | Description |
| --- | --- |
| `Task<TOut> IfTrueOrElseAsync<TOut>(this bool condition, Func<Task<TOut>> thenFunc, Func<Task<TOut>> elseFunc)` | Asynchronously evaluates one of two functions based on the boolean value and returns the result. |
| `Task IfTrueOrElseAsync(this bool condition, ...)` | Asynchronously executes one of two actions based on the boolean value. |

Overloads also extend `Task<bool>` so a boolean-producing task can be branched directly.

---

## FunctionalAsyncExtensions

Async versions of the general-purpose functional extension methods (Alt, Fork, Pipe, IterateUntil). Enables async pipeline composition and iterative computations with async update functions.

| Member | Description |
| --- | --- |
| `Task<TOut?> AltAsync<TIn, TOut>(this Task<TIn> instance, params Func<TIn, Task<TOut>>[] args)` | Asynchronously applies each function to the awaited value, returning the first non-null result. |
| `Task<TOut> ForkAsync<TIn, T1, T2, TOut>(...)` | Applies two independent async functions to the same input, then combines their results. |
| `Task ForkAsync<TIn, T1, T2>(...)` | Applies two independent async functions to the same input, then combines their results via an action. |
| `Task<TResult> PipeAsync<T, TResult>(this Task<T> instance, Func<T, Task<TResult>> func)` | Passes the awaited value through an async transformation function. |
| `Task<T> PipeAsync<T>(this Task<T> instance, Func<T, Task> action)` | Executes an async side-effect on the awaited value and returns it unchanged. |
| `Task<T> IterateUntilAsync<T>(...)` | Repeatedly applies an async update function until the end condition is met. |
| `Task<Result<T>> IterateUntilAsync<T>(...)` | Repeatedly applies a failable async update function until the end condition is met or a failure occurs; returns the final Result. |
