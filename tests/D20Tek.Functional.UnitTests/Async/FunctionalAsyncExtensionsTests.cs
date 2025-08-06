using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class FunctionalAsyncExtensionsTests
{
    [TestMethod]
    public async Task ForkAsync_WithTwoInputFuncs_ReturnsCombinedOutput()
    {
        // arrange
        double[] grades = [4, 3.5, 3.7, 3.3, 4.3];
        var task = Task.FromResult(grades);

        // act
        var result = await task.ForkAsync(
            g => Task.FromResult(g.Sum()),
            g => Task.FromResult(g.Count()),
            (totalPoints, numClasses) => Task.FromResult(Math.Round(totalPoints / numClasses, 2)));

        result.Should().Be(3.76);
    }

    [TestMethod]
    public async Task ForkAsync_WithTwoInputFuncs_RunsAction()
    {
        // arrange
        double[] grades = [4, 3.5, 3.7, 3.3, 4.3];
        var task = Task.FromResult(grades);
        string output = string.Empty;
        Func<double, int, Task> changeOutput = (total, num) =>
        {
            output = Math.Round(total / num, 2).ToString();
            return Task.CompletedTask;
        };

        // act
        await task.ForkAsync(
            g => Task.FromResult(g.Sum()),
            g => Task.FromResult(g.Count()),
            (totalPoints, numClasses) => changeOutput(totalPoints, numClasses));

        output.Should().Be("3.76");
    }

    [TestMethod]
    public async Task IterateUntilAsync_RunUpdateFunction_UntilEndCondition()
    {
        // arrange
        var count = 0;
        var task = Task.FromResult(count);

        // act
        var result = await task.IterateUntilAsync(
            x => Task.FromResult(x += 1),
            x => Task.FromResult(x > 5));

        // assert
        result.Should().Be(6);
    }

    [TestMethod]
    public async Task IterateUntilAsync_UpdateThrowsException_LogsError()
    {
        // arrange
        var count = 0;
        var task = Task.FromResult(count);

        // act
        await Assert.ThrowsAsync<DivideByZeroException>([ExcludeFromCodeCoverage]() => task.IterateUntilAsync(
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x / 0),
            x => Task.FromResult(x > 5)));
    }

    [TestMethod]
    public async Task IterateUntilAsync_RunUpdateResultFunction_UntilEndCondition()
    {
        // arrange
        var count = 0;
        var task = Task.FromResult(count);

        // act
        var result = await task.IterateUntilAsync(
            x => Task.FromResult(Result<int>.Success(x += 1)),
            x => Task.FromResult(x > 5));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(6);
    }

    [TestMethod]
    public async Task IterateUntilAsync_RunUpdateFunction_ReturnsSuccess()
    {
        // arrange
        var count = 0;
        var task = Task.FromResult(count);

        // act
        var result = await task.IterateUntilAsync(
            x => Task.FromResult(Result<int>.Failure(Error.Unexpected("code", "text"))),
            x => Task.FromResult(x > 5));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task IterateUntilAsync_RunThrowsException_ReturnsFailure()
    {
        // arrange
        var count = 0;
        var task = Task.FromResult(count);

        // act
        var result = await task.IterateUntilAsync(
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(Result<int>.Success(x / 0)),
            x => Task.FromResult(x > 5));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task AltAsync_WithTwoInputFuncs_ReturnsThroughAll()
    {
        // arrange
        var text = "foo";
        var task = Task.FromResult(text);

        // act
        var result = await task.AltAsync(
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x == "bar" ? "it was bar" : null),
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x == "foo" ? "it was foo" : null));

        // assert
        result.Should().Be("it was foo");
    }

    [TestMethod]
    public async Task AltAsync_WithTwoInputFuncs_ReturnsFirstPath()
    {
        // arrange
        var text = "bar";
        var task = Task.FromResult(text);

        // act
        var result = await task.AltAsync(
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x == "bar" ? "it was bar" : null),
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x == "foo" ? "it was foo" : null));

        // assert
        result.Should().Be("it was bar");
    }

    [TestMethod]
    public async Task AltAsync_WithNoMatches_ReturnsNull()
    {
        // arrange
        var text = "other";
        var task = Task.FromResult(text);

        // act
        var result = await task.AltAsync(
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x == "bar" ? "it was bar" : null),
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(x == "foo" ? "it was foo" : null));

        // assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task PipeAsync_WithFunc_ReturnsChainedResult()
    {
        // arrange
        Func<int, int, Task<int>> add = (x, y) => Task.FromResult(x + y);

        // act
        var result = await add(1, 3).PipeAsync(z => add(z, 5));

        // assert
        result.Should().Be(9);
    }

    [TestMethod]
    public async Task PipeAsync_WithFunc_ReturnsChainedResult2()
    {
        // arrange
        Func<string, string, Task<string>> add = (x, y) => Task.FromResult($"{x} {y}");

        // act
        var result = await add("test", "with").PipeAsync(z => add(z, "string"));

        // assert
        result.Should().Be("test with string");
    }

    [TestMethod]
    public async Task PipeAsync_WithAction_ReturnsChainedResult2()
    {
        // arrange
        var output = "test";
        var task = Task.FromResult(output);
        Func<string, Task> append = (x) => { output += $" {x}"; return Task.CompletedTask; };

        // act
        var result = await task.PipeAsync(_ => append("with")).PipeAsync(_ => append("string"));

        // assert
        output.Should().Be("test with string");
    }
}
