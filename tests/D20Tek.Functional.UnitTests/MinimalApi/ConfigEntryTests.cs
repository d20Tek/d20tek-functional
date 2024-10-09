using D20Tek.Functional.AspNetCore;

namespace D20Tek.Functional.UnitTests.MinimalApi;

[TestClass]
public class ConfigEntryTests
{
    [TestMethod]
    public void UpdateWith_ReturnsNewEntry()
    {
        // arrange
        var entry = new ConfigEntry(3, System.Net.HttpStatusCode.NotFound);

        // act
        var result = entry with { ErrorType = 4, StatusCode = System.Net.HttpStatusCode.BadRequest };

        // assert
        result.ErrorType.Should().Be(4);
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
