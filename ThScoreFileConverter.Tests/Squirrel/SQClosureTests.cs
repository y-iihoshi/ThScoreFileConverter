using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Tests.Squirrel;

[TestClass]
public class SQClosureTests
{
    [TestMethod]
    public void SQClosureTest()
    {
        var closure = new SQClosure();

        closure.Type.ShouldBe(SQObjectType.Closure);
    }

    internal static SQClosure CreateTestHelper(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        return SQClosure.Create(reader);
    }

    [TestMethod]
    public void CreateTest()
    {
        var sqclosure = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Closure));

        sqclosure.Type.ShouldBe(SQObjectType.Closure);
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Should.Throw<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null)));
    }
}
