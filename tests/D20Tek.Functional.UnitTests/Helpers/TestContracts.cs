namespace D20Tek.Functional.UnitTests.Helpers;

[ExcludeFromCodeCoverage]
public sealed record TestRequest(int Id);

[ExcludeFromCodeCoverage]
public sealed record TestResponse(int Id, string Message);

[ExcludeFromCodeCoverage]
public sealed record TestEntity(int Id, string Message, DateTime CreatedDate);

public sealed class TestController : ControllerBase
{
}
