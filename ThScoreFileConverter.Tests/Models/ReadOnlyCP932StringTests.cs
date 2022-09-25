using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ReadOnlyCP932StringTests
{
    [TestMethod]
    public void ReadOnlyCP932StringTest()
    {
        var expectedBytes = TestUtils.CP932Encoding.GetBytes("博麗 霊夢\0霧雨 魔理沙");
        var actual = new ReadOnlyCP932String(expectedBytes);

        Assert.AreEqual("博麗 霊夢", actual.ToString());
        Assert.AreNotEqual("博麗 霊夢\0霧雨 魔理沙", actual.ToString());
        CollectionAssert.That.AreEqual(expectedBytes, actual.Bytes);
    }

    [TestMethod]
    public void EmptyTest()
    {
        var empty = ReadOnlyCP932String.Empty;
        Assert.IsFalse(empty.Bytes.Any());
        Assert.AreEqual(string.Empty, empty.ToString());
    }

    [TestMethod]
    public void EmptyTestSameInstances()
    {
        var empty = ReadOnlyCP932String.Empty;
        var empty2 = ReadOnlyCP932String.Empty;
        Assert.AreSame(empty, empty2);
    }
}
