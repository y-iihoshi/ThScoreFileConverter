using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQFloatTests
{
    [TestMethod]
    public void SQFloatTest()
    {
        var sqfloat = new SQFloat();

        sqfloat.Type.ShouldBe(SQObjectType.Float);
        sqfloat.Value.ShouldBe(0f);
        ((float)sqfloat).ShouldBe(0f);
    }

    internal static SQFloat CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQFloat.Create(reader);
    }

    [DataTestMethod]
    [DataRow(0f)]
    [DataRow(1f)]
    [DataRow(-1f)]
    [DataRow(0.25f)]
    [DataRow(0.1f)]
    [DataRow(float.NaN)]
    [DataRow(float.PositiveInfinity)]
    [DataRow(float.NegativeInfinity)]
    public void CreateTest(float expected)
    {
        var sqfloat = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Float, expected));

        sqfloat.Type.ShouldBe(SQObjectType.Float);
        sqfloat.Value.ShouldBe(expected);
        ((float)sqfloat).ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestShortened()
    {
        _ = Should.Throw<EndOfStreamException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Float, new byte[3])));
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, 0f)));
    }

    [TestMethod]
    public void EqualsTestNull()
    {
        new SQFloat().Equals(null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestNullObject()
    {
        new SQFloat().Equals((object)null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestInvalidType()
    {
        new SQFloat().Equals(SQNull.Instance).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestSelf()
    {
        var value = new SQFloat();

        value.Equals(value).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestSelfObject()
    {
        var value = new SQFloat();

        value.Equals(value as object).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestEqual()
    {
        new SQFloat().Equals(new SQFloat(0f)).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestNotEqual()
    {
        new SQFloat().Equals(new SQFloat(1f)).ShouldBeFalse();
    }

    [TestMethod]
    public void GetHashCodeEqual()
    {
        new SQFloat(0f).GetHashCode().ShouldBe(new SQFloat().GetHashCode());
    }

    [TestMethod]
    public void GetHashCodeNotEqual()
    {
        Assert.AreNotEqual(new SQFloat().GetHashCode(), new SQFloat(1f).GetHashCode());
    }

    [TestMethod]
    public void MyTestMethod()
    {
        new SQFloat(1.2f).ToString().ShouldBe("1.2");
    }
}
