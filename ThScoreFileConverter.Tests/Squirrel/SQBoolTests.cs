using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQBoolTests
{
    [TestMethod]
    public void SQBoolTestTrue()
    {
        var sqbool = SQBool.True;

        sqbool.Type.ShouldBe(SQObjectType.Bool);
        sqbool.Value.ShouldBeTrue();
        ((bool)sqbool).ShouldBeTrue();
    }

    [TestMethod]
    public void SQBoolTestFalse()
    {
        var sqbool = SQBool.False;

        sqbool.Type.ShouldBe(SQObjectType.Bool);
        sqbool.Value.ShouldBeFalse();
        ((bool)sqbool).ShouldBeFalse();
    }

    internal static SQBool CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQBool.Create(reader);
    }

    [TestMethod]
    [DataRow((byte)0x00, false)]
    [DataRow((byte)0x01, true)]
    [DataRow((byte)0x02, true)]
    [DataRow((byte)0xFF, true)]
    public void CreateTest(byte serialized, bool expected)
    {
        var sqbool = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool, serialized));

        sqbool.Type.ShouldBe(SQObjectType.Bool);
        sqbool.Value.ShouldBe(expected);
        ((bool)sqbool).ShouldBe(expected);
    }

    [TestMethod]
    public void CreateTestShortened()
    {
        _ = Should.Throw<EndOfStreamException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool)));
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, (byte)0x00)));
    }

    [TestMethod]
    public void EqualsTestNull()
    {
        SQBool.True.Equals(null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestNullObject()
    {
        SQBool.True.Equals((object)null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestInvalidType()
    {
        SQBool.True.Equals(SQNull.Instance).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestSelf()
    {
        SQBool.True.Equals(SQBool.True).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestSelfObject()
    {
        SQBool.True.Equals(SQBool.True as object).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestEqual()
    {
        var sqtrue = CreateTestHelper(SquirrelHelper.MakeByteArray(true));

        SQBool.True.Equals(sqtrue).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestNotEqual()
    {
        SQBool.True.Equals(SQBool.False).ShouldBeFalse();
    }

    [TestMethod]
    public void GetHashCodeTestEqual()
    {
        var sqtrue = CreateTestHelper(SquirrelHelper.MakeByteArray(true));

        sqtrue.GetHashCode().ShouldBe(SQBool.True.GetHashCode());
    }

    [TestMethod]
    public void GetHashCodeTestNotEqual()
    {
        SQBool.True.GetHashCode().ShouldNotBe(SQBool.False.GetHashCode());
    }

    [TestMethod]
    public void ToStringTest()
    {
        SQBool.True.ToString().ShouldBe("True");
        SQBool.False.ToString().ShouldBe("False");
    }
}
