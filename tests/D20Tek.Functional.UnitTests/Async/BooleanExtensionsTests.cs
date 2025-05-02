using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class BooleanAsyncExtensionsTests
{
    [TestMethod]
    public async Task IfTrueOrElseAsyncFunc_WithPositiveCondition_ReturnsThenFunc()
    {
        // arrange
        var x = 42;

        // act
        var result = await (x > 5).IfTrueOrElseAsync(
            () => Task.FromResult("thenFunc"),
            [ExcludeFromCodeCoverage] () => Task.FromResult("elseFunc"));

        // assert
        result.Should().Be("thenFunc");
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncFunc_WithNegativeCondition_ReturnsElseFunc()
    {
        // arrange
        var x = 4;

        // act
        var result = await (x > 5).IfTrueOrElseAsync(
            [ExcludeFromCodeCoverage] () => Task.FromResult("thenFunc"),
            () => Task.FromResult("elseFunc"));

        // assert
        result.Should().Be("elseFunc");
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncAction_WithPositiveCondition_CallsThenAction()
    {
        // arrange
        var x = 42;
        var result = 0;
        Func<Task> thenAction = () => Task.FromResult(result += 1);
        Func<Task> elseAction = [ExcludeFromCodeCoverage] () => Task.FromResult(result -= 1);

        // act
        await(x > 5).IfTrueOrElseAsync(thenAction, elseAction);

        // assert
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncAction_WithPositiveConditionNoElse_CallsThenAction()
    {
        // arrange
        var x = 42;
        var result = 0;
        Func<Task> thenAction = () => Task.FromResult(result += 1);

        // act
        await (x > 5).IfTrueOrElseAsync(thenAction);

        // assert
        result.Should().Be(1);
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncAction_WithNegativeCondition_CallsElseAction()
    {
        // arrange
        var x = 4;
        var result = 0;
        Func<Task> thenAction = [ExcludeFromCodeCoverage] () => Task.FromResult(result += 1);
        Func<Task> elseAction = () => Task.FromResult(result -= 1);

        // act
        await (x > 5).IfTrueOrElseAsync(thenAction, elseAction);

        // assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncAction_WithNegativeConditionNoElse_MakesNoCall()
    {
        // arrange
        var x = 4;
        var result = 0;
        Func<Task> thenAction = [ExcludeFromCodeCoverage] () => Task.FromResult(result += 1);

        // act
        await (x > 5).IfTrueOrElseAsync(thenAction);

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncFunc_WithAsyncAndPositiveCondition_ReturnsThenFunc()
    {
        // arrange
        var x = 42;
        Task<bool> task = Task.FromResult(x > 5);

        // act
        var result = await task.IfTrueOrElseAsync(
            () => Task.FromResult("thenFunc"),
            [ExcludeFromCodeCoverage] () => Task.FromResult("elseFunc"));

        // assert
        result.Should().Be("thenFunc");
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncFunc_WithAsyncAndNegativeCondition_ReturnsElseFunc()
    {
        // arrange
        var x = 4;
        Task<bool> task = Task.FromResult(x > 5);

        // act
        var result = await task.IfTrueOrElseAsync(
            [ExcludeFromCodeCoverage] () => Task.FromResult("thenFunc"),
            () => Task.FromResult("elseFunc"));

        // assert
        result.Should().Be("elseFunc");
    }

    [TestMethod]
    public async Task IfTrueOrElseAsyncAction_WithAsyncAndPositiveCondition_CallsThenAction()
    {
        // arrange
        var x = 42;
        var task = Task.FromResult(x > 5);
        var result = 0;
        Func<Task> thenAction = () => Task.FromResult(result += 1);
        Func<Task> elseAction = [ExcludeFromCodeCoverage] () => Task.FromResult(result -= 1);

        // act
        await task.IfTrueOrElseAsync(thenAction, elseAction);

        // assert
        result.Should().Be(1);
    }
}
