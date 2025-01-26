using System.Collections;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ReadOnlyCP932BytesTests
{
    [TestMethod]
    public void ReadOnlyCP932StringTest()
    {
        var expectedBytes = TestUtils.CP932Encoding.GetBytes("博麗 霊夢\0霧雨 魔理沙");
        var actual = new ReadOnlyCP932Bytes(expectedBytes);

        Assert.AreEqual("博麗 霊夢", actual.ToString());
        Assert.AreNotEqual("博麗 霊夢\0霧雨 魔理沙", actual.ToString());
        CollectionAssert.That.AreEqual(expectedBytes, actual.Bytes);
    }

    [TestMethod]
    public void EmptyTest()
    {
        var empty = ReadOnlyCP932Bytes.Empty;
        Assert.IsFalse(empty.Bytes.Any());
        Assert.AreEqual(string.Empty, empty.ToString());
    }

    [TestMethod]
    public void EmptyTestSameInstances()
    {
        var empty = ReadOnlyCP932Bytes.Empty;
        var empty2 = ReadOnlyCP932Bytes.Empty;
        Assert.AreSame(empty, empty2);
    }

    [TestMethod]
    public void GetEnumeratorTestGeneric()
    {
        var expectedBytes = TestUtils.CP932Encoding.GetBytes("博麗 霊夢\0霧雨 魔理沙");
        var actual = new ReadOnlyCP932Bytes(expectedBytes);

        var index = 0;
        foreach (var actualByte in actual)
        {
            Assert.AreEqual(expectedBytes[index++], actualByte);
        }
    }

    [TestMethod]
    public void GetEnumeratorTest()
    {
        var expectedBytes = TestUtils.CP932Encoding.GetBytes("博麗 霊夢\0霧雨 魔理沙");
        var actual = new ReadOnlyCP932Bytes(expectedBytes);

        var index = 0;
        foreach (var actualByte in actual as IEnumerable)
        {
            Assert.AreEqual(expectedBytes[index++], actualByte);
        }
    }
}
