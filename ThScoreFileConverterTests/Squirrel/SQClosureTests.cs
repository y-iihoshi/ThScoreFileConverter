using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.UnitTesting;

namespace ThScoreFileConverterTests.Squirrel;

[TestClass]
public class SQClosureTests
{
    [TestMethod]
    public void SQClosureTest()
    {
        var closure = new SQClosure();

        Assert.AreEqual(SQObjectType.Closure, closure.Type);
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

        Assert.AreEqual(SQObjectType.Closure, sqclosure.Type);
    }

    [TestMethod]
    public void CreateTestInvalid()
    {
        _ = Assert.ThrowsException<InvalidDataException>(
            () => CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null)));
    }
}
