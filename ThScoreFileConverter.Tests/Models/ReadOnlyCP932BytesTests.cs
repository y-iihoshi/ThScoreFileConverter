using System.Collections;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverter.Tests.Models;

[TestClass]
public class ReadOnlyCP932BytesTests
{
    [TestMethod]
    public void ReadOnlyCP932StringTest()
    {
        var expectedBytes = TestUtils.CP932Encoding.GetBytes("博麗 霊夢\0霧雨 魔理沙");
        var actual = new ReadOnlyCP932Bytes(expectedBytes);

        actual.ToString().ShouldBe("博麗 霊夢");
        actual.ToString().ShouldNotBe("博麗 霊夢\0霧雨 魔理沙");
        actual.Bytes.ShouldBe(expectedBytes);
    }

    [TestMethod]
    public void EmptyTest()
    {
        var empty = ReadOnlyCP932Bytes.Empty;
        empty.Bytes.Any().ShouldBeFalse();
        empty.ToString().ShouldBeEmpty();
    }

    [TestMethod]
    public void EmptyTestSameInstances()
    {
        var empty = ReadOnlyCP932Bytes.Empty;
        var empty2 = ReadOnlyCP932Bytes.Empty;
        empty2.ShouldBeSameAs(empty);
    }

    [TestMethod]
    public void GetEnumeratorTestGeneric()
    {
        var expectedBytes = TestUtils.CP932Encoding.GetBytes("博麗 霊夢\0霧雨 魔理沙");
        var actual = new ReadOnlyCP932Bytes(expectedBytes);

        var index = 0;
        foreach (var actualByte in actual)
        {
            actualByte.ShouldBe(expectedBytes[index++]);
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
            actualByte.ShouldBe(expectedBytes[index++]);
        }
    }
}
