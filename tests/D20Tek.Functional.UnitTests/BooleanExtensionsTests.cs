namespace D20Tek.Functional.UnitTests;

[TestClass]
public class BooleanExtensionsTests
{
    [TestMethod]
    public void IfTrueOrElseFunc_WithPositiveCondition_ReturnsThenFunc()
    {
        // arrange
        var x = 42;

        // act
        var result = (x > 5).IfTrueOrElse(() => "thenFunc", [ExcludeFromCodeCoverage] () => "elseFunc");

        // assert
        result.Should().Be("thenFunc");
    }

    [TestMethod]
    public void IfTrueOrElseFunc_WithNegativeCondition_ReturnsElseFunc()
    {
        // arrange
        var x = 4;

        // act
        var result = (x > 5).IfTrueOrElse([ExcludeFromCodeCoverage] () => "thenFunc", () => "elseFunc");

        // assert
        result.Should().Be("elseFunc");
    }

    [TestMethod]
    public void IfTrueOrElseAction_WithPositiveCondition_CallsThenAction()
    {
        // arrange
        var x = 42;
        var result = 0;
        Action thenAction = () => result += 1;
        Action elseAction = [ExcludeFromCodeCoverage] () => result -= 1;

        // act
        (x > 5).IfTrueOrElse(thenAction, elseAction);

        // assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void IfTrueOrElseAction_WithPositiveConditionNoElse_CallsThenAction()
    {
        // arrange
        var x = 42;
        var result = 0;
        Action thenAction = () => result += 1;

        // act
        (x > 5).IfTrueOrElse(thenAction);

        // assert
        result.Should().Be(1);
    }

    [TestMethod]
    public void IfTrueOrElseAction_WithNegativeCondition_CallsElseAction()
    {
        // arrange
        var x = 4;
        var result = 0;
        Action thenAction = [ExcludeFromCodeCoverage] () => result += 1;
        Action elseAction = () => result -= 1;

        // act
        (x > 5).IfTrueOrElse(thenAction, elseAction);

        // assert
        result.Should().Be(-1);
    }

    [TestMethod]
    public void IfTrueOrElseAction_WithNegativeConditionNoElse_MakesNoCall()
    {
        // arrange
        var x = 4;
        var result = 0;
        Action thenAction = [ExcludeFromCodeCoverage] () => result += 1;

        // act
        (x > 5).IfTrueOrElse(thenAction);

        // assert
        result.Should().Be(0);
    }
}
