namespace D20Tek.Functional.UnitTests;

[TestClass]
public class IEnumerableExtensionsTests
{
    [TestMethod]
    public void AsString_WithStringItems_ReturnsConcatenatedString()
    {
        // arrange
        List<string> list = ["test1", "test2", "test3"];

        // act
        var result = list.AsString();

        // assert
        result.Should().Be("test1, test2, test3");
    }

    [TestMethod]
    public void AsString_WithNoItems_ReturnsDefaultString()
    {
        // arrange
        List<string> list = [];

        // act
        var result = list.AsString("none.");

        // assert
        result.Should().Be("none.");
    }

    [TestMethod]
    public void AsString_WithIntItems_ReturnsConcatenatedString()
    {
        // arrange
        int[] list = [1, 2, 3, 42];

        // act
        var result = list.AsString();

        // assert
        result.Should().Be("1, 2, 3, 42");
    }

    internal class Foo
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [ExcludeFromCodeCoverage]
        public string Value { get; set; } = string.Empty;

        public override string ToString() { return $"{Name} (id: {Id})"; }
    }

    [TestMethod]
    public void AsString_WithComplexItems_ReturnsConcatenatedToString()
    {
        // arrange
        List<Foo> list = [
            new Foo { Id = 1, Name = "Foo", Value = "other" },
            new Foo { Id = 8, Name = "Bar", Value = "another" },
            new Foo { Id = 11, Name = "Baz", Value = "another one" } ];

        // act
        var result = list.AsString();

        // assert
        result.Should().Be("Foo (id: 1), Bar (id: 8), Baz (id: 11)");
    }

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
