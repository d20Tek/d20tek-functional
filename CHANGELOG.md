# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.13] - Result aggregation

### Added

- `Result.Combine` applicative-style aggregation helpers that evaluate every input and collect all errors instead of short-circuiting on the first failure. Overloads include `Combine<T>(params Result<T>[])` returning `Result<T[]>`, plus typed overloads for two through five results that project their success values through a combiner function.
- `Result.Traverse<T, TResult>(IEnumerable<T>, Func<T, Result<TResult>>)` maps each element through a failable selector and collects the outcomes, and `Result.Sequence<T>(IEnumerable<Result<T>>)` collapses a sequence of results into a single result.
- Async counterparts in `D20Tek.Functional.Async` on `ResultCombineAsyncExtensions`: `CombineAsync` (array and two- through five-result typed overloads), `TraverseAsync`, and `SequenceAsync`. Each awaits all supplied tasks before aggregating so errors from every input are collected.

### Changed

- The non-generic `Result` factory class is now declared `partial` so the aggregation helpers can be defined in a separate file.
- Updated package references to the latest versions.

## [1.1.12] - Memoization

### Added

- `Memoize<T, TResult>` extension methods that cache the results of pure functions keyed by their input arguments, using `ConcurrentDictionary<T, Lazy<TResult>>` internally for thread-safe, single-evaluation caching. Overloads are provided for functions with one through five parameters, consistent with other multi-arity types in this library (e.g. `Choice`). 
- An async variant, `MemoizeAsync<T, TResult>`, is also provided in `D20Tek.Functional.Async`, sharing in-flight tasks across concurrent calls with the same arguments and evicting failed results so later calls can retry.

### Changed

- Updated package references to the latest versions.

## [1.1.10] - LINQ query syntax

### Added

- Added full documentation set for this library, including introduction, getting started, and multiple Api reference documents.
- LINQ query syntax support for `Optional<T>` and `Result<T>`. New `OptionalLinqExtensions` and `ResultLinqExtensions` provide `Select`, `SelectMany`, and `Where`, enabling `from ... select ...` comprehension syntax. These extensions live in the main `D20Tek.Functional` namespace so query syntax works with the same `using` that imports the core types. `Result<T>.Where` takes an explicit `Error` argument so a rejected value never produces a hidden or invented error.
