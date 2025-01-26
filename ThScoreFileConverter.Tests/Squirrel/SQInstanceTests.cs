using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQInstanceTests
{
    [TestMethod]
    public void SQInstanceTest()
    {
        var instance = new SQInstance();

        Assert.AreEqual(SQObjectType.Instance, instance.Type);
    }

    internal static SQInstance CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQInstance.Create(reader);
    }

    [TestMethod]
    public void CreateTest()
    {
        var sqinstance = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Instance));

        Assert.AreEqual(SQObjectType.Instance, sqinstance.Type);
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null)));
    }
}
