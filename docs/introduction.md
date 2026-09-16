# Introduction

If you have ever chased a `NullReferenceException` through five layers of call stack, wrapped half your service in try/catch just to translate exceptions into HTTP status codes, or lost the thread of a method buried in nested `if` checks, this library is for you. Functionally (aka D20Tek.Functional) gives you the functional building blocks C# still lacks, so you can write code where absence, failure, and branching are explicit in the type system instead of lurking at runtime.

The core `D20Tek.Functional` package provides the primitives - `Optional<T>`, `Result<T>`, `Choice<T1, T2>`, `Identity<T>`, and more - plus the composition operators (`Map`, `Bind`, `Iter`, `Match`) that let you chain them into readable pipelines. The companion `D20Tek.Functional.AspNetCore` package takes it to the edge of your app, turning a `Result<T>` into an `IResult` or `ActionResult<T>` without the usual boilerplate.

I played a lot with different functional styles and this library was the result of that experimentation. It is opinionated, but the opinions are based on what I have found to be the most practical and maintainable way to apply functional programming in C#. There are other libraries that provide similar funtionality, but this was my attempt to learn functional programming concepts. I have used this library and many of my own projects and continue expanding it as I run into now scenarios or cases.

Discriminated unions are coming to C# soon. That will change the capabilities of this library... hopefully some of it will go away. But some of the concepts, like Optional and Result will likely survive in my codebase (and with many other developers).

The API is modeled on F#'s functional core, so if you already work in F# the concepts and method names will feel familiar, and if you do not, you get a gentle on-ramp to the same ideas from within C#.

## Why this matters in your day-to-day code

Functional programming favors expressions over statements, immutable data over mutable state, and explicit data flow over hidden side effects. That is not academic. It is the difference between bugs the compiler catches and bugs your users find.

Consider a typical lookup-and-transform. Here is the imperative version, with its silent failure modes:

```csharp
public string GetDisplayName(int id)
{
    var user = _repository.Find(id);   // might return null
    if (user == null)
    {
        return "(unknown)";            // easy to forget this branch
    }

    return user.Name.ToUpperInvariant();
}
```

And here is the same intent expressed as a pipeline, where the empty case cannot be forgotten because the type forces you to handle it:

```csharp
public string GetDisplayName(int id) =>
    _repository.Find(id)                       // returns Optional<User>
        .Map(user => user.Name.ToUpperInvariant())
        .DefaultValue("(unknown)");
```

The payoff shows up across the whole codebase:

- **Fewer null reference bugs.** Replacing `null` with an explicit `Optional<T>` makes absence part of the type. The compiler and your `Match` handlers force you to consider the empty case instead of discovering it in production.
- **Errors as values, not exceptions.** With `Result<T>` you model success and failure as data. Chain a sequence of operations with `Bind` and `Map`, and any failure short-circuits the pipeline while carrying structured `Error` information forward. No exceptions for control flow, no losing context on the way up the stack.
- **Predictable, composable pipelines.** Small, pure transformations combine into larger workflows. Operators like `Pipe`, `Fork`, and `Alt` let you express intent directly instead of through nested conditionals and temporary variables.
- **Tests you actually want to write.** Pure functions that take inputs and return outputs, without reaching into shared mutable state, are straightforward to test in isolation and need far less mocking.
- **Effects where you can see them.** `Iter` and the `Unit` type isolate side effects such as logging and persistence at the edges of a pipeline, keeping the core logic pure and easy to follow.

## What you get in the box

C# has picked up plenty of functional features over the years - LINQ, lambdas, pattern matching, records, and expression-bodied members - but it still lacks first-class discriminated unions and a standard option/result type. This library fills those gaps so you do not have to hand-roll them per project:

- **Option monad** - `Optional<T>` provides a type-safe alternative to `null`, with `Some`/`None`, safe accessors (`DefaultValue`, `DefaultWith`, `Match`), and conversions to and from nullable references.
- **Result monad** - `Result<T>` represents either a success value or an array of structured `Error` values, enabling exception-free error handling and railway-oriented flows.
- **Discriminated unions** - `Choice<T1, T2>` (and its async variants) model a value that is legitimately one of several types, matched exhaustively rather than with type checks.
- **Supporting monads and helpers** - `Identity<T>` lifts plain values into pipelines, the `State` extensions support immutable state transitions, `ValidationErrors` accumulates multiple validation failures, and `TryExcept` turns try/catch/finally into expressions.
- **General combinators** - extension methods such as `Pipe`, `Fork`, `Alt`, `IterateUntil`, `IfTrueOrElse`, and `ForEach` bring a functional style to any type.
- **LINQ query syntax** - `Optional<T>` and `Result<T>` implement `Select` and `SelectMany` (and `Where`), so you can compose them with the `from ... select ...` syntax you already know.
- **First-class async** - the `D20Tek.Functional.Async` namespace mirrors the synchronous operators with `MatchAsync`, `BindAsync`, `MapAsync`, and `IterAsync`, plus `Task` helpers like `ThenAsync`, so asynchronous code composes just as cleanly.
- **ASP.NET Core integration** - `D20Tek.Functional.AspNetCore` converts `Result<T>` into Minimal API `IResult` or MVC `ActionResult<T>` responses, maps `Error` values to RFC 7807 Problem Details, and provides filters that handle results automatically.

Enumerables are intentionally left to LINQ. Because `IEnumerable<T>` already follows a functional paradigm in C#, the library does not duplicate `Map`/`Bind`/`Iter` over collections to avoid confusion between similarly named methods.

## Learn by reading real code

Documentation only goes so far. This project ships an extensive set of complete, runnable applications - not isolated snippets - so you can see how the types combine in practice. Clone the repo, open a sample, and step through it:

- **Console applications** (`samples/Apps`) - TipCalc, GpaCalc, UnitConverter, GeneratePassword, a reusable TerminalAppTemplate, and the BudgetTracker and WealthTracker apps that show command-driven, result-based workflows.
- **Games** (`samples/Games`) - ChutesAndLadders and MartianTrail, which illustrate functional state transitions and pipeline-driven game logic. These were inspired by "Functional Programming in C#" by Simon J. Painter.
- **Blazor WebAssembly apps** (`samples/Blazor`) - BudgetTracker and WealthTracker, demonstrating functional patterns with async persistence in an interactive UI.
- **Web APIs** (`samples/WebApi`) - MemberService and TodoService, showing how `D20Tek.Functional.AspNetCore` turns `Result<T>` outcomes into clean HTTP responses and Problem Details.

Browsing these samples is the fastest way to see how the individual types combine into idiomatic, end-to-end functional C#.

## Start here

1. Read [Getting Started](getting-started.md) to install the packages and write your first result-based workflow.
2. Keep the API references open as you build:
   - [Core API Reference](api-reference-core.md) - the primary types in `D20Tek.Functional`.
   - [Async API Reference](api-reference-async.md) - the `D20Tek.Functional.Async` namespace.
   - [ASP.NET Core API Reference](api-reference-aspnetcore.md) - the `D20Tek.Functional.AspNetCore` package.
3. Pick the sample closest to what you are building and adapt it.

The next time a `null` slips through or an exception hijacks your control flow, you will have a better tool to apply to that job.
