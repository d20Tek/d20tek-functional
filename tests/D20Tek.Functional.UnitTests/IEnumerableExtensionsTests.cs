namespace D20Tek.Functional.UnitTests;

[TestClass]
public class IEnumerableExtensionsTests
{
    [TestMethod]
    public void ForEach_WithItems_PerformsActionToEach()
    {
        // arrange
        IEnumerable<string> strings = ["test ", "this ", "func."];
        var output = string.Empty;

        // act
        strings.ForEach(s => output += s);

        // assert
        output.Should().Be("test this func.");
    }

    [TestMethod]
    public void ForEach_WithNoItems_DoesNothing()
    {
        // arrange
        IEnumerable<string> strings = [];
        var output = "none";

        // act
        strings.ForEach([ExcludeFromCodeCoverage] (s) => output += s);

        // assert
        output.Should().Be("none");
    }
}
