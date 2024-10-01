namespace D20Tek.Functional.UnitTests;

[TestClass]
public class IdentityTests
{
    [TestMethod]
    public void Get_WithCreatedValue_ReturnsValue()
    {
        // arrange
        var v = Identity<int>.Create(13);

        // act
        var result = v.Get();

        // assert
        result.Should().Be(13);
        v.ToString().Should().Be("Identity<Int32>(value = 13)");
        v.Count().Should().Be(1);
    }

    [TestMethod]
    public void Bind_WithValue_ReturnsIdentity()
    {
        // arrange
        var id = Identity<string>.Create("42");

        // act
        var result = id.Bind(v => TryParse(v));

        // assert
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsDefaultIdentity()
    {
        // arrange
        var id = Identity<string>.Create("non-int-text");

        // act
        int result = id.Bind(v => TryParse(v));

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void Contains_WithEqualValue_ReturnsTrue()
    {
        // arrange
        var item = Identity<string>.Create("foo");

        // act
        bool contains = item.Contains("foo");

        // assert
        contains.Should().BeTrue();
    }

    [TestMethod]
    public void Contains_WithUnequalValue_ReturnsFalse()
    {
        // arrange
        var item = Identity<string>.Create("foo");

        // act
        bool contains = item.Contains("bar");

        // assert
        contains.Should().BeFalse();
    }

    [TestMethod]
    public void Exists_WithPositivePredicate_ReturnsTrue()
    {
        // arrange
        var item = Identity<int>.Create(42);

        // act
        var result = item.Exists(x => x > 10);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WithNegativePredicate_ReturnsFalse()
    {
        // arrange
        var item = Identity<int>.Create(42);

        // act
        var result = item.Exists(x => x == 10);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Iter_WithValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var option = Identity<string>.Create("Hello World");

        // act
        option.Iter(x => output = x);

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public void Map_WithValue_ReturnsNewIdentity()
    {
        // arrange
        var option = Identity<int>.Create(42);

        // act
        var result = option.Map(v => v * 2);

        // assert
        result.Get().Should().Be(84);
    }

    public static Identity<int> TryParse(string text) =>
        int.TryParse(text, out int parsed) ? parsed : 0;
}
