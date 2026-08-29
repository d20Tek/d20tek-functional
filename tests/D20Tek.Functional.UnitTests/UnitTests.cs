namespace D20Tek.Functional.UnitTests;

[TestClass]
public class UnitTests
{
    [TestMethod]
    public void Value_ReturnsDefaultUnit()
    {
        // arrange / act
        var unit = Unit.Value;

        // assert
        unit.Should().Be(default(Unit));
    }

    [TestMethod]
    public void Equals_WithAnotherUnit_ReturnsTrue()
    {
        // arrange
        var a = Unit.Value;
        var b = Unit.Value;

        // act / assert
        a.Equals(b).Should().BeTrue();
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
    }

    [TestMethod]
    public void Equals_WithObject_ReturnsExpected()
    {
        // arrange
        var unit = Unit.Value;

        // act / assert
        unit.Equals((object)Unit.Value).Should().BeTrue();
        unit.Equals("not a unit").Should().BeFalse();
    }

    [TestMethod]
    public void GetHashCode_ReturnsZero()
    {
        // arrange / act / assert
        Unit.Value.GetHashCode().Should().Be(0);
    }

    [TestMethod]
    public void ImplicitConversion_FromValueTuple_ReturnsUnit()
    {
        // arrange / act
        Unit unit = default(ValueTuple);

        // assert
        unit.Should().Be(Unit.Value);
    }

    [TestMethod]
    public void ToString_ReturnsFSharpConvention()
    {
        // arrange / act / assert
        Unit.Value.ToString().Should().Be("()");
    }

    [TestMethod]
    public void ResultSuccess_WithNoValue_ReturnsSuccessfulUnitResult()
    {
        // arrange / act
        var result = Result.Success();

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(Unit.Value);
    }

    [TestMethod]
    public void ResultSuccess_WithValue_ReturnsSuccessfulResult()
    {
        // arrange / act
        var result = Result.Success(42);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void ResultFailure_WithError_ReturnsFailedUnitResult()
    {
        // arrange
        var error = Error.Validation("Test.Error", "test error");

        // act
        var result = Result.Failure(error);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().ContainSingle().Which.Should().Be(error);
    }

    [TestMethod]
    public void ResultFailure_WithErrorArray_ReturnsFailedUnitResult()
    {
        // arrange
        var errors = new[]
        {
            Error.Validation("Test.Error1", "test error 1"),
            Error.Validation("Test.Error2", "test error 2")
        };

        // act
        var result = Result.Failure(errors);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().BeEquivalentTo(errors);
    }

    [TestMethod]
    public void ResultFailure_WithException_ReturnsFailedUnitResult()
    {
        // arrange
        var ex = new InvalidOperationException("boom");

        // act
        var result = Result.Failure(ex);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().ContainSingle();
    }
}
