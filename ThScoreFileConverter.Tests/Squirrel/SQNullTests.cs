using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQNullTests
{
    [TestMethod]
    public void InstanceTest()
    {
        var sqnull = SQNull.Instance;

        sqnull.Type.ShouldBe(SQObjectType.Null);
    }

    internal static SQNull CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQNull.Create(reader);
    }

    [TestMethod]
    public void CreateTest()
    {
        var sqnull = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null));

        sqnull.Type.ShouldBe(SQObjectType.Null);
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Bool)));
    }

    [TestMethod]
    public void EqualsTestNull()
    {
        SQNull.Instance.Equals(null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestNullObject()
    {
        SQNull.Instance.Equals((object)null!).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestInvalidType()
    {
        SQNull.Instance.Equals(SQBool.True).ShouldBeFalse();
    }

    [TestMethod]
    public void EqualsTestSelf()
    {
        SQNull.Instance.Equals(SQNull.Instance).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTestSelfObject()
    {
        SQNull.Instance.Equals(SQNull.Instance as object).ShouldBeTrue();
    }

    [TestMethod]
    public void EqualsTest()
    {
        var created = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null));

        SQNull.Instance.Equals(created).ShouldBeTrue();
    }

    [TestMethod]
    public void GetHashCodeTest()
    {
        var created = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null));

        created.GetHashCode().ShouldBe(SQNull.Instance.GetHashCode());
    }

    [TestMethod]
    public void ToStringTest()
    {
        SQNull.Instance.ToString().ShouldBe("Null");
    }
}
