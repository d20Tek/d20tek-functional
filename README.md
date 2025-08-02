# D20Tek.Functional

## Introduction

---
**Warning:** I am in the process of making breaking changes to the Option&lt;T&gt; class to rename it to Optional&lt;T&gt;. Option is used in many unrelated places, including the .NET runtime itself. So to reduce confusion, we will use Optional&lt;T&gt;.

The current version (0.9.17) has support for both Option&lt;T&gt; and Optional&lt;T&gt; to give developers a chance to update their code to use Optional&lt;T&gt;. The next version will be a major version update and remove the Option&lt;T&gt; class altogether.
---

Welcome to D20Tek.Functional code library, this package contains a set of classes that improve the functional programming experience in C#. I based many of the library classes and api on F#'s functional capabilities to ensure smooth usage between the two: classes like Option, Result, and Choice; and methods like Iter, Bind, and Map. I took inspiration for this library (and a couple of the sample games - ChutesAndLadders and MartianTrail) from "Functional Programming in C#" by Simon J Painter (https://github.com/madSimonJ/FunctionalProgrammingWithCSharp).

I added the D20Tek.Functional.AspNetCore library to this project as well. It wraps some common functionality to transform Result<T> objects into appropriate service endpoint responses, or ProblemDetails for errors that occur. This library removes a lot of repetitive code for dealing with results in WebApi development. It's a separate package to minimize dependencies on the base package, and only add that overhead if you need to integrate with ASP.NET.

As of the first version, I did not provide extension methods for arrays, lists, or enumerables that extend the same functional methods (like Iter, Bind, Map, etc). Enumerables in C# already follow a functional paradigm using LINQ, so I didn't want to add confusion by adding methods that do similar things, but with different names. If I get feedback requests to provide those as well, then I will look at incorporating an extension layer on IEnumerables.

There is also an extensive set of sample console applications and simple games that show how to use this package.

## Installation
This library is a NuGet package so it is easy to add to your project. To install the package into your solution, you can use the NuGet Package Manager. In PM, please use the following command:

```cmd
PM > Install-Package D20Tek.Functional -Version 0.9.17
PM > Install-Package D20Tek.Functional.AspNetCore -Version 0.9.17
```

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.Functional, and install whichever packages you require from there.

Note: This package is still in pre-release because to ensure that the API works cleanly in multiple scenarios. Once it's been used in several projects and the API solidifies, it will move to a stable release.

## Usage
Once you've installed the NuGet package, you can start using it in your .NET projects.

Code examples coming soon...

## Samples

## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository. I'm still in the process of building these libraries and samples, so any suggestions that would make it more useable are welcome.
