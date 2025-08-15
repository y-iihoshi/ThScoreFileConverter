using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQIntegerTests
{
    [TestMethod]
    public void SQIntegerTest()
    {
        var sqinteger = new SQInteger();

        sqinteger.Type.ShouldBe(SQObjectType.Integer);
        sqinteger.Value.ShouldBe(0);
        ((int)sqinteger).ShouldBe(0);
    }

    internal static SQInteger CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQInteger.Create(reader);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(-1)]
    [DataRow(int.MaxValue)]
    [DataRow(int.MinValue)]
    public void CreateTest(int expected)
    {
        var sqinteger = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Integer, expected));

        sqinteger.Type.ShouldBe(SQObjectType.Integer);
        sqinteger.Value.ShouldBe(expected);
        ((int)sqinteger).ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestShortened()
    {
        _ = Should.Throw<EndOfStreamException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Integer, new byte[3])));
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, 123)));
    }

    [TestMethod]
    public void EqualsTestNull()
    {
        new SQInteger().Equals(null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestNullObject()
    {
        new SQInteger().Equals((object)null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestInvalidType()
    {
        new SQInteger().Equals(SQNull.Instance).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestSelf()
    {
        var value = new SQInteger();

        value.Equals(value).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestSelfObject()
    {
        var value = new SQInteger();

        value.Equals(value as object).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestEqual()
    {
        new SQInteger().Equals(new SQInteger(0)).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestNotEqual()
    {
        new SQInteger().Equals(new SQInteger(1)).ShouldBeFalse();
    }

    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        new SQInteger(0).GetHashCode().ShouldBe(new SQInteger().GetHashCode());
    }

    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        new SQInteger(1).GetHashCode().ShouldNotBe(new SQInteger().GetHashCode());
    }

    [TestMethod]
    public void ToStringTest()
    {
        new SQInteger(12).ToString().ShouldBe("12");
    }
}
