# Getting Started

This guide walks you through installing the Functionally packages and writing your first result-based workflows in C#.

## Prerequisites

- .NET 9 or later.
- A C# project (console, ASP.NET Core, Blazor, or class library).

## Installation

Functionally ships as NuGet packages. Install the core package, and add the ASP.NET Core companion only if you need HTTP integration.

Using the Package Manager Console:

```cmd
PM > Install-Package D20Tek.Functional
PM > Install-Package D20Tek.Functional.AspNetCore
```

Using the .NET CLI:

```cmd
dotnet add package D20Tek.Functional
dotnet add package D20Tek.Functional.AspNetCore
```

You can also install both packages from the Visual Studio NuGet Package Manager UI by searching for "D20Tek.Functional".

## Your first Optional

`Optional<T>` replaces `null` with an explicit, type-safe representation of an optional value.

```csharp
using D20Tek.Functional;

Optional<string> FindUserName(int id) =>
	id == 1 ? Optional<string>.Some("Ada") : Optional<string>.None();

string display = FindUserName(1)
	.Map(name => name.ToUpperInvariant())
	.DefaultValue("(unknown)");

// display == "ADA"
```

Use `Match` to handle both cases explicitly:

```csharp
string message = FindUserName(42).Match(
	onSome: name => $"Found {name}",
	onNone: () => "No user found");
```

## Your first Result

`Result<T>` models an operation that either succeeds with a value or fails with one or more structured `Error` values.

```csharp
using D20Tek.Functional;

Result<decimal> ParseAmount(string input) =>
	decimal.TryParse(input, out var amount)
		? Result<decimal>.Success(amount)
		: Result<decimal>.Failure(Error.Validation("Amount.Invalid", "Amount is not a valid number."));

Result<decimal> ApplyDiscount(decimal amount) =>
	amount >= 0
		? Result<decimal>.Success(amount * 0.9m)
		: Result<decimal>.Failure(Error.Validation("Amount.Negative", "Amount cannot be negative."));
```

Chain dependent operations with `Bind` (railway-oriented programming). If any step fails, the errors flow through unchanged:

```csharp
string summary = ParseAmount("100")
	.Bind(ApplyDiscount)
	.Map(final => $"Total: {final:C}")
	.Match(
		onSuccess: text => text,
		onFailure: errors => errors.AsString());
```

## Side-effect-only operations with Unit

When an operation succeeds but has no meaningful value to return, use `Result<Unit>` and the non-generic `Result` factory helpers:

```csharp
using D20Tek.Functional;

Result<Unit> SaveChanges()
{
	// ... perform the side effect ...
	return Result.Success();          // successful Result<Unit>
	// return Result.Failure(Error.Unexpected("Save.Failed", "Could not save."));
}
```

## Accumulating validation errors

Use `ValidationErrors` to collect every failure rather than stopping at the first one:

```csharp
using D20Tek.Functional;

Result<Order> Validate(string name, int quantity) =>
	ValidationErrors.Create()
		.AddIfError(() => string.IsNullOrWhiteSpace(name), "Order.Name", "Name is required.")
		.AddIfError(() => quantity <= 0, "Order.Quantity", "Quantity must be positive.")
		.Map(() => new Order(name, quantity));
```

## Composing with LINQ query syntax

If you prefer LINQ comprehension syntax, `Optional<T>` and `Result<T>` support `Select` and `SelectMany`, so you can compose them with `from ... select ...`. These extensions live in the main `D20Tek.Functional` namespace, so the same `using` that brings in the types also enables query syntax.

```csharp
using D20Tek.Functional;

Result<UserAccountDto> LoadAccount(int id) =>
	from user in GetUser(id)              // Result<User>
	from account in GetAccount(user.Id)   // Result<Account>
	select new UserAccountDto(user.Name, account.Balance);
```

If any step returns a failure (or `None` for `Optional<T>`), the query short-circuits and the failure flows through unchanged. `Optional<T>` also supports the `where` clause:

```csharp
Optional<int> firstEven =
	from n in ParseNumber(input)
	where n % 2 == 0
	select n;
```

For `Result<T>`, filtering requires an explicit error to represent the rejected value, so `Result<T>.Where` takes an `Error` argument and is called directly rather than through the `where` clause:

```csharp
Result<int> positive = ParseAmount(input)
	.Where(n => n > 0, Error.Validation("Amount.Positive", "Amount must be positive."));
```

## Working asynchronously

The `D20Tek.Functional.Async` namespace mirrors the synchronous operators for `Task`-based code:

```csharp
using D20Tek.Functional;
using D20Tek.Functional.Async;

Task<Result<string>> LoadAsync(int id) => /* ... */;

Task<string> pipeline = LoadAsync(1)
	.BindAsync(async value => await ProcessAsync(value))
	.MapAsync(value => value.ToUpperInvariant())
	.MatchAsync(
		onSuccess: value => value,
		onFailure: errors => errors.AsString());
```

## Returning results from ASP.NET Core

With `D20Tek.Functional.AspNetCore`, convert a `Result<T>` directly into an HTTP response. In a Minimal API:

```csharp
using D20Tek.Functional.AspNetCore.MinimalApi;

app.MapGet("/orders/{id}", (int id, IOrderService service) =>
	service.GetOrder(id).ToApiResult());
```

In an MVC controller:

```csharp
using D20Tek.Functional.AspNetCore.WebApi;

[HttpGet("{id}")]
public ActionResult<OrderResponse> Get(int id) =>
	_service.GetOrder(id).ToActionResult(o => o.ToResponse(), this);
```

Failures are automatically translated into RFC 7807 Problem Details, with the HTTP status code derived from each `Error`'s type.

## Explore the samples

The `samples` folder contains complete, runnable applications that demonstrate these patterns end to end:

- `samples/Apps` - console applications (TipCalc, GpaCalc, UnitConverter, BudgetTracker, WealthTracker, and more).
- `samples/Games` - ChutesAndLadders and MartianTrail.
- `samples/Blazor` - BudgetTracker and WealthTracker Blazor WebAssembly apps.
- `samples/WebApi` - MemberService and TodoService using the ASP.NET Core integration.

## Next steps

- [Core API Reference](api-reference-core.md) - the primary types in `D20Tek.Functional`.
- [Async API Reference](api-reference-async.md) - the `D20Tek.Functional.Async` namespace.
- [ASP.NET Core API Reference](api-reference-aspnetcore.md) - the `D20Tek.Functional.AspNetCore` package.
