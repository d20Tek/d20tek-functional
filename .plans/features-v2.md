# D20Tek.Functional v2 – Potential Features

## Potentially Missing Functional Concepts

| Concept | Description | Priority | Fit for Library |
|---------|-------------|----------|-----------------|
| **`Unit`** | A proper void replacement type (like F#'s `unit`). Allows `Result<Unit>` for side-effect operations instead of needing a dummy value. | High | High – very common pain point |
| **LINQ Query Syntax** | Implementing `Select`/`SelectMany` on `Optional<T>` and `Result<T>` enables `from x in result select ...` syntax. | High | High – beloved by C#/LINQ users |
| **`Result.Combine` / `Traverse`** | Combine multiple Results into one (applicative). A general `Result[]` → `Result<T[]>` combinator. | Medium-High | High – common ask for applicative patterns |
| **`Option.OfNullable<T>` / `OfObj<T>`** | Factory methods to bridge nullable reference types into `Optional<T>`. | Medium | Medium – quality-of-life |
| **`Memoize<T>`** | Caches pure function results for referential transparency. | Medium | Medium – simple but useful utility |
| **`Either<TLeft, TRight>`** | Unbiased two-value type (vs. Result which is biased toward success). | Low | Low – `Choice<T1,T2>` already fills this role |
| **`Tap` / `Tee`** | Side-effect in a pipeline without altering the value. | Low | Low – `Iter` and `Pipe` with action already cover this |
| **Reader Monad** | Dependency injection / environment threading in a functional style. | Low | Low – less common in C# FP libraries |

---

## Implementation Details

### 1. `Unit` Type [Done]

A zero-size struct representing "no meaningful value" — the functional equivalent of `void`.

**Why it matters:** C# generics cannot use `void` as a type parameter. Without `Unit`, developers needing `Result<T>` for side-effect-only operations must invent a dummy type or use `Result<bool>` with a meaningless value.

**Implementation approach:**
- Create a `readonly record struct Unit` with a single static `Value` property.
- Add an implicit conversion from `ValueTuple` so `default` works naturally.
- Override `ToString()` to return `"()"` (F# convention).
- Add `Result<Unit>` convenience factory methods (e.g., `Result.Success()` without a type parameter).
- Add `Optional<Unit>` support if meaningful.

**Example usage:**
```csharp
// Before: awkward dummy value
Result<bool> DoWork() => Result<bool>.Success(true);

// After: expressive intent
Result<Unit> DoWork() => Result<Unit>.Success(Unit.Value);
```

**Files to create/modify:**
- `src/D20Tek.Functional/Unit.cs` (new)
- `src/D20Tek.Functional/Result.cs` (add convenience overloads)
- Unit tests for `Unit` type

---

### 2. LINQ Query Syntax Support [Done]

Implement `Select` and `SelectMany` on `Optional<T>` and `Result<T>` to enable LINQ comprehension syntax.

**Why it matters:** C# developers are deeply familiar with LINQ. Supporting query syntax makes monadic composition feel native rather than requiring method-chain fluency with `Bind`/`Map`.

**Implementation notes:**
- `Select`/`SelectMany`/`Where` live in the main `D20Tek.Functional` namespace (not a separate `Linq` namespace) so query syntax compiles with the same `using` that brings in the core types.
- `Optional<T>.Where` delegates to `Filter`. `Result<T>.Where` takes an explicit `Error` argument (it cannot be used via the parameterless `where` clause) so a rejected value never produces a hidden/invented error.

**Implementation approach:**
- Add `Select<T, TResult>(this Optional<T>, Func<T, TResult>)` — delegates to `Map`.
- Add `SelectMany<T, TIntermediate, TResult>(this Optional<T>, Func<T, Optional<TIntermediate>>, Func<T, TIntermediate, TResult>)` — delegates to `Bind` + `Map`.
- Mirror the same pattern for `Result<T>`.
- Optionally add `Where` on `Optional<T>` (delegates to `Filter`).

**Example usage:**
```csharp
// Monadic composition via LINQ
var result = from user in GetUser(id)
			 from account in GetAccount(user.AccountId)
			 select new UserAccountDto(user.Name, account.Balance);
```

**Files to create/modify:**
- `src/D20Tek.Functional/OptionalLinqExtensions.cs` (new)
- `src/D20Tek.Functional/ResultLinqExtensions.cs` (new)
- Async variants if needed
- Unit tests for LINQ query patterns

---

### 3. `Result.Combine` / `Traverse`

Aggregate multiple `Result<T>` values into a single `Result<T[]>`, collecting all errors on failure (applicative style).

**Why it matters:** Real-world code often validates multiple fields or calls multiple services. Without `Combine`, developers must manually aggregate errors or short-circuit on the first failure.

**Implementation approach:**
- `Result.Combine(params Result<T>[] results)` → `Result<T[]>`: returns `Success` with all values if all succeed, or `Failure` with all collected errors.
- `Result.Combine(Result<T1>, Result<T2>, ..., Func<T1,T2,...,TOut>)` overloads for typed composition (up to ~5 type parameters).
- `Traverse<T, TResult>(IEnumerable<T>, Func<T, Result<TResult>>)` → `Result<IEnumerable<TResult>>`: maps and collects.
- Consider how this relates to the existing `ValidationErrors` type — `Combine` is the general-purpose version.

**Example usage:**
```csharp
var name = ValidateName(input.Name);
var email = ValidateEmail(input.Email);
var age = ValidateAge(input.Age);

var result = Result.Combine(name, email, age, (n, e, a) => new User(n, e, a));
// If any fail, all errors are collected into the failure
```

**Files to create/modify:**
- `src/D20Tek.Functional/ResultCombineExtensions.cs` (new)
- Async variants in `src/D20Tek.Functional/Async/`
- Unit tests for combine/traverse

---

### 4. `Optional.OfNullable<T>` / `OfObj<T>`

Factory methods that bridge nullable reference and value types into `Optional<T>`.

**Why it matters:** Interop with existing .NET APIs that return `T?` (nullable references) or `Nullable<T>` (nullable value types) is a constant friction point. These factories make the bridge explicit and safe.

**Implementation approach:**
- `Optional.OfNullable<T>(T? value) where T : struct` → returns `Some` if `HasValue`, else `None`.
- `Optional.OfObj<T>(T? value) where T : class` → returns `Some` if non-null, else `None`.
- Consider adding `ToOptional()` extension methods on `T?` for discoverability.

**Example usage:**
```csharp
int? maybeAge = GetAge();
Optional<int> optAge = Optional.OfNullable(maybeAge);

string? maybeName = GetName();
Optional<string> optName = Optional.OfObj(maybeName);
```

**Files to create/modify:**
- `src/D20Tek.Functional/Optional.cs` (add static factory methods)
- `src/D20Tek.Functional/OptionalExtensions.cs` (add `ToOptional()` extensions)
- Unit tests

---

### 5. `Memoize<T>`

A utility that caches the result of a pure function based on its input arguments.

**Why it matters:** Memoization is a fundamental FP optimization. Providing a built-in helper avoids developers rolling their own thread-unsafe caches.

**Implementation approach:**
- `Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)` using `ConcurrentDictionary<T, Lazy<TResult>>` internally for thread safety.
- Overloads for `Func<T1, T2, TResult>` etc. (up to ~3 parameters).
- Consider an async variant `Func<T, Task<TResult>> MemoizeAsync<T, TResult>(...)`.

**Example usage:**
```csharp
Func<int, int> fib = null!;
fib = ((Func<int, int>)(n => n <= 1 ? n : fib(n - 1) + fib(n - 2))).Memoize();
var result = fib(40); // fast due to caching
```

**Files to create/modify:**
- `src/D20Tek.Functional/MemoizeExtensions.cs` (new)
- `src/D20Tek.Functional/Async/MemoizeAsyncExtensions.cs` (new, optional)
- Unit tests
