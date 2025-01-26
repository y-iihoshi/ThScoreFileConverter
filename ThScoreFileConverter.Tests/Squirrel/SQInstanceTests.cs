using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQInstanceTests
{
    [TestMethod]
    public void SQInstanceTest()
    {
        var instance = new SQInstance();

        instance.Type.ShouldBe(SQObjectType.Instance);
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

        sqinstance.Type.ShouldBe(SQObjectType.Instance);
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null)));
    }
}
