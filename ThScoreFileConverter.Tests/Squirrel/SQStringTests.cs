using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQStringTests
{
    [TestMethod]
    public void SQStringTest()
    {
        var sqstring = new SQString();

        sqstring.Type.ShouldBe(SQObjectType.String);
        sqstring.Value.ShouldBeEmpty();
        ((string)sqstring).ShouldBeEmpty();
    }

    internal static SQString CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQString.Create(reader);
    }

    [TestMethod]
    [DataRow("abc")]
    [DataRow("博麗 霊夢")]
    [DataRow("")]
    [DataRow("\0")]
    public void CreateTest(string expected)
    {
        var bytes = TestUtils.CP932Encoding.GetBytes(expected);
        var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, bytes.Length, bytes));

        sqstring.Type.ShouldBe(SQObjectType.String);
        sqstring.Value.ShouldBe(expected);
        ((string)sqstring).ShouldBe(expected);
    }

    [TestMethod]
    [DataRow(0, "")]
    [DataRow(0, "abc")]
    [DataRow(0, null)]
    [DataRow(-1, "")]
    [DataRow(-1, "abc")]
    [DataRow(-1, null)]
    public void CreateTestEmpty(int size, string value)
    {
        var bytes = (value is null) ? [] : TestUtils.CP932Encoding.GetBytes(value);
        var sqstring = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, size, bytes));

        sqstring.Type.ShouldBe(SQObjectType.String);
        sqstring.Value.ShouldBeEmpty();
        ((string)sqstring).ShouldBeEmpty();
    }

    [TestMethod]
    [DataRow("abc")]
    [DataRow("博麗 霊夢")]
    public void CreateTestShortened(string value)
    {
        var bytes = TestUtils.CP932Encoding.GetBytes(value);
        _ = Should.Throw<EndOfStreamException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.String, bytes.Length + 1, bytes)));
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, 3, "abc")));
    }

    [TestMethod]
    public void EqualsTestNull()
    {
        new SQString().Equals(null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestNullObject()
    {
        new SQString().Equals((object)null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestInvalidType()
    {
        new SQString().Equals(SQNull.Instance).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestSelf()
    {
        var value = new SQString();

        value.Equals(value).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestSelfObject()
    {
        var value = new SQString();

        value.Equals(value as object).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestEqual()
    {
        new SQString(string.Empty).Equals(new SQString()).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestNotEqual()
    {
        new SQString("博麗 霊夢").Equals(new SQString()).ShouldBeFalse();
    }

    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        new SQString(string.Empty).GetHashCode().ShouldBe(new SQString().GetHashCode());
    }

    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        new SQString("博麗 霊夢").GetHashCode().ShouldNotBe(new SQString().GetHashCode());
    }

    [TestMethod]
    public void ToStringTest()
    {
        new SQString("博麗 霊夢").ToString().ShouldBe("博麗 霊夢");
    }

    [TestMethod]
    public void ToStringTestEmpty()
    {
        new SQString().ToString().ShouldBeEmpty();
    }
}
