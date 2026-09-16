# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.10] - LINQ query syntax

### Added

- Added full documentation set for this library, including introduction, getting started, and multiple Api reference documents.
- LINQ query syntax support for `Optional<T>` and `Result<T>`. New `OptionalLinqExtensions` and `ResultLinqExtensions` provide `Select`, `SelectMany`, and `Where`, enabling `from ... select ...` comprehension syntax. These extensions live in the main `D20Tek.Functional` namespace so query syntax works with the same `using` that imports the core types. `Result<T>.Where` takes an explicit `Error` argument so a rejected value never produces a hidden or invented error.
