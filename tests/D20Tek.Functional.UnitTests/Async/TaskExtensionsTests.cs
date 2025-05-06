using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class TaskExtensionsTests
{
    [TestMethod]
    public async Task ThenAsync_WithSameOutType_ReturnsTask()
    {
        // arrange
        var task = Task.FromResult(10);

        // act
        var resultingTask = task.ThenAsync(x => Task.FromResult(x + 5));
        var result = await resultingTask;

        // assert
        resultingTask.Should().BeAssignableTo<Task<int>>();
        result.Should().Be(15);
    }

    [TestMethod]
    public async Task ThenAsync_WithDiffOutType_ReturnsTaskTOut()
    {
        // arrange
        var task = Task.FromResult(10);

        // act
        var resultingTask = task.ThenAsync(x => Task.FromResult(x.ToString()));
        var result = await resultingTask;

        // assert
        resultingTask.Should().BeAssignableTo<Task<string>>();
        result.Should().Be("10");
    }
}
